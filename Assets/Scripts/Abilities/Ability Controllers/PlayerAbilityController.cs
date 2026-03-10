using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace AbilitySystem {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerAbilityController : AbilityController {

        [Space]
        [Header("Transition Animation")]
        [SerializeField] Animation SmokesAndMirrorsAnimation;
        [SerializeField] AbilityAnimation transitionAnimationClip;

        [Space]
        [Header("Abilities")]
        [SerializeField] List<PlayerSetSummary> playerSetsList;
        [HideInInspector] public PlayerSetSummary currentAbilitySet;
        private CharacterController characterController;

        #region OnStart
        protected override void Awake() {
            base.Awake();
            characterController = GetComponent<CharacterController>();

            transitionAnimationClip.Setup(SmokesAndMirrorsAnimation, WrapMode.Once);
            foreach (var i in playerSetsList) {
                if (i.abilitySetSO == null)
                    continue;
                i.playerAbilitySet = new PlayerAbilitySet(i.abilitySetSO, i.entityBody.animationComponent);
                i.entityBody.iAbility = this;
            }
            currentAbilitySet = playerSetsList[0];
        }
        public override void SetupAnimations() {
            foreach (var n in playerSetsList)
                n.abilitySetSO?.SetupAnimations(n.entityBody.animationComponent);
        }
        #endregion

        #region Transitions
        public void SetAbility(string name) {
            if (!playerSetsList.Any(x => x.abilitySetSO.abilitySetName == name)) {
                Debug.LogWarning("No ability set of that name.");
                return;
            }
            currentAbilitySet = playerSetsList.First(x => x.abilitySetSO.abilitySetName == name);
        }
        #endregion

        #region Movement Functions
        public override void OnMoveEntity(Vector3 direction, float turnSpeed) {
            characterController.Move(direction);
            direction.y = 0;
            if (direction != Vector3.zero) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
        }
        public override void OnRotateEntity(Vector3 direction) {
            direction.y = 0;
            if (direction != Vector3.zero) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
        }
        #endregion

        #region Input Functions
        public override void InputMove(Vector3 moveInput, bool isRunning) =>
            currentAbilitySet.
            playerAbilitySet?.
            movement.Activate(
                currentAbilitySet.entityBody,
                moveInput,
                isRunning);
        public override void InputPrimaryAttack() =>
            currentAbilitySet.playerAbilitySet?.light.Activate(currentAbilitySet.entityBody);

        public override void InputPrimaryAbility() =>
            currentAbilitySet.playerAbilitySet?.primary.Activate(currentAbilitySet.entityBody);
        #endregion

        #region Data
        public override EntityBody GetEntityBody() => currentAbilitySet.entityBody;

        [Serializable]
        public class PlayerSetSummary {
            public PlayerAbilitySetSO abilitySetSO;
            public EntityBody entityBody;
            [HideInInspector] public PlayerAbilitySet playerAbilitySet;
        }
        #endregion
    }
}