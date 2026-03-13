using System;
using System.Collections;
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
            SmokesAndMirrorsAnimation.gameObject.SetActive(false);
            characterController = GetComponent<CharacterController>();
            transitionAnimationClip.Setup(SmokesAndMirrorsAnimation, WrapMode.Once);
            foreach (var i in playerSetsList) {
                if (i.abilitySetSO == null)
                    continue;
                i.playerAbilitySet = new PlayerAbilitySet(i.abilitySetSO, i.entityBody.animationComponent);
                i.entityBody.iAbility = this;
                i.entityBody.modelPrefab.SetActive(false);
            }
            currentAbilitySet = playerSetsList[0];
            currentAbilitySet.entityBody.modelPrefab.SetActive(true);
        }
        public override void SetupAnimations() {
            foreach (var n in playerSetsList)
                n.abilitySetSO?.SetupAnimations(n.entityBody.animationComponent);
        }
        #endregion

        #region Transitions
        public override void InputTransitionName(string name) {

            if (!playerSetsList.Any(x => x.abilitySetSO.abilitySetName == name)) {
                Debug.LogWarning("No ability set of that name.");
                return;
            }

            PlayerSetSummary checkAbilitySet = playerSetsList.First(x => x.abilitySetSO.abilitySetName == name);
            if (checkAbilitySet == currentAbilitySet) {
                Debug.LogWarning("Currently set to this set.");
                return;
            }

            StartCoroutine(RunAnimationsWithEvents(
                new TimelineEvent[] {
                    new TimelineEvent(currentAbilitySet.entityBody.animationComponent, currentAbilitySet.abilitySetSO.transitionAnimation, 0, 1),
                    new TimelineEvent(checkAbilitySet.entityBody.animationComponent, checkAbilitySet.abilitySetSO.transitionAnimation, 1, 2),
                    new TimelineEvent(SmokesAndMirrorsAnimation, transitionAnimationClip, 0, 2, true)
                },
                new DeltaEvent[] {
                    new DeltaEvent(() => {
                        canUseAbilities = false; SmokesAndMirrorsAnimation.gameObject.SetActive(true);
                    }, 0),
                    new DeltaEvent(() => {
                        currentAbilitySet.entityBody.modelPrefab.SetActive(false);
                        checkAbilitySet.entityBody.modelPrefab.SetActive(true);
                        currentAbilitySet = checkAbilitySet;
                    }, 0.5f),
                    new DeltaEvent(() => {
                        canUseAbilities = true;
                        SmokesAndMirrorsAnimation.gameObject.SetActive(false);
                    }, 1)
                }
            ));
            print("End");
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
        public override void InputMove(Vector3 moveInput, bool isRunning) {
            base.InputMove(moveInput, isRunning);
            currentAbilitySet.
            playerAbilitySet?.
            movement.Activate(
                currentAbilitySet.entityBody,
                moveInput,
                isRunning);
        }
        public override void InputPrimaryAttack() {
            base.InputPrimaryAttack();
            currentAbilitySet.playerAbilitySet?.light.Activate(currentAbilitySet.entityBody);
        }

        public override void InputPrimaryAbility() {
            base.InputPrimaryAbility();
            currentAbilitySet.playerAbilitySet?.primary.Activate(currentAbilitySet.entityBody);
        }
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