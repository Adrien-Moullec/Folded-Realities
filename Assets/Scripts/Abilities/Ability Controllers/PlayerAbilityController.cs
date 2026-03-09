using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


namespace AbilitySystem
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerAbilityController : AbilityController
    {

        [Space]
        [Header("Abilities")]
        [SerializeField] List<PlayerSetSummary> playerSetsList;
        [HideInInspector] internal PlayerSetSummary currentAbilitySet;
        private CharacterController characterController;

        private Vector3 currentViewDir = Vector3.zero;

        #region OnStart
        protected override void Awake()
        {
            base.Awake();
            characterController = GetComponent<CharacterController>();

            for (int i = 0; i < playerSetsList.Count; i++)
            {
                PlayerSetSummary summary = playerSetsList[i];
                if (summary.abilitySetSO == null)
                    continue;
                summary.playerAbilitySet = new PlayerAbilitySet(summary.abilitySetSO, summary.entityBody.animationComponent);
                summary.entityBody.iAbility = this;
                if (summary.switchAnimation.animation != null) summary.entityBody.animationComponent.AddClip(currentAbilitySet.switchAnimation.animation, currentAbilitySet.switchAnimation.animation.name);
            }
            currentAbilitySet = playerSetsList[0];
        }
        internal override void SetupAnimations()
        {
            foreach (var n in playerSetsList)
                n.abilitySetSO?.SetupAnimations(n.entityBody.animationComponent);
        }
        #endregion
        public void SetAbility(string name)
        {
            if (playerSetsList.Any(x => x.abilitySetSO.abilitySetName == name))
                currentAbilitySet = playerSetsList.First(x => x.abilitySetSO.abilitySetName == name);
            else
                Debug.LogWarning("No ability to set of name " + name);
        }

        #region Input Functions

        public override void InputMove(Vector3 moveInput, bool isRunning) =>
            currentAbilitySet.
            playerAbilitySet?.
            movement.Activate(
                currentAbilitySet.entityBody,
                moveInput,
                isRunning);
        public override void InputPrimaryAttack()
        {
            currentAbilitySet.playerAbilitySet?.light.Activate(currentAbilitySet.entityBody);
        }
        public override void InputPrimaryAbility() =>
            throw new NotImplementedException();
        #endregion

        #region Movement Functions

        public override void OnMoveEntity(Vector3 direction, float turnSpeed)
        {
            characterController.Move(direction);
            direction.y = 0;
            if (direction != Vector3.zero) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
            //Vector3.RotateTowards(entityBody.bodyHolder.transform.forward, direction, turnSpeed, 0);
        }
        public override void OnRotateEntity(Vector3 movement)
        {
            throw new NotImplementedException();
        }

        #endregion

        [Serializable]
        public class PlayerSetSummary
        {
            public PlayerAbilitySetSO abilitySetSO;
            public EntityBody entityBody;
            public AbilityAnimation switchAnimation;
            [HideInInspector] public PlayerAbilitySet playerAbilitySet;
        }
    }
}