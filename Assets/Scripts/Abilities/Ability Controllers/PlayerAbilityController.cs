using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace AbilitySystem
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerAbilityController : AbilityController
    {

        [Space]
        [Header("Abilities")]
        [SerializeField] List<PlayerSetForController> playerSetsList;
        [HideInInspector] List<PlayerAbilitySet> abilitySetList = new();
        [HideInInspector] internal PlayerAbilitySet currentAbilitySet;
        private CharacterController characterController;


        private Vector3 currentViewDir = Vector3.zero;

        #region OnStart
        protected override void Awake()
        {
            base.Awake();
            characterController = GetComponent<CharacterController>();

            foreach (var n in playerSetsList)
            {
                if (n.abilitySet == null)
                    continue;
                PlayerAbilitySet ab = new PlayerAbilitySet(n.abilitySet, n.entityBody.animationComponent);
                abilitySetList.Add(ab);
            }
            currentAbilitySet = abilitySetList[0];
        }
        internal override void SetupAnimations()
        {
            foreach (var n in playerSetsList)
                n.abilitySet?.SetupAnimations(n.entityBody.animationComponent);
        }
        #endregion

        private void TestPlayAnimations(AbilityAnimation abilityAnimation, AnimationStyle style)
        {
            switch (style)
            {
                case AnimationStyle.Play: entityBody.animationComponent.CrossFade(abilityAnimation.animation.name, abilityAnimation.crossFadeTime); break;
                case AnimationStyle.Queue: entityBody.animationComponent.CrossFadeQueued(abilityAnimation.animation.name); break;
            }
        }

        public void SetAbility(string name)
        {
            if (abilitySetList.Any(x => x.abilitySetName == name))
                currentAbilitySet = abilitySetList.First(x => x.abilitySetName == name);
            else
                Debug.LogWarning("No ability to set of name " + name);
        }

        #region Input Functions

        public override void InputMove(Vector3 moveInput, bool isRunning) =>
            currentAbilitySet?.movement.Activate(entityBody, moveInput, isRunning);
        public override void InputPrimaryAttack()
        {
            currentAbilitySet?.light.Activate(entityBody);
        }
        public override void InputPrimaryAbility() =>
            throw new NotImplementedException();
        #endregion

        #region Movement Functions

        public override void OnMoveEntity(Vector3 direction, float turnSpeed)
        {
            characterController.Move(direction);
            direction.y = 0;
            if (direction != Vector3.zero) entityBody.bodyHolder.transform.forward = direction;
            //Vector3.RotateTowards(entityBody.bodyHolder.transform.forward, direction, turnSpeed, 0);
        }
        public override void OnRotateEntity(Vector3 movement)
        {
            throw new NotImplementedException();
        }

        #endregion

        [Serializable]
        public struct PlayerSetForController
        {
            public PlayerAbilitySetSO abilitySet;
            public EntityBody entityBody;
        }
    }
}