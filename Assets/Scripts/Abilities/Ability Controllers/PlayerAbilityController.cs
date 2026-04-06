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
        [SerializeField] float transitionTime = 0.5f;
        private bool isTransitioning = false;

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
            foreach (var i in playerSetsList) {
                if (i.abilitySetSO == null)
                    continue;

                i.entityBody.iAbility = this;
                i.entityBody.modelPrefab.SetActive(false);
                i.entityBody.iHealth = this;

                i.playerAbilitySet = new PlayerAbilitySet(i.abilitySetSO, i.entityBody);
                i.playerAbilitySet.movement.AbilityData = playerSetsList[0].playerAbilitySet.movement.AbilityData;

                if (i.playerAbilitySet.movement?.movementSO != null)
                    frameEvents += i.playerAbilitySet.movement.FrameEvent;
                if (i.playerAbilitySet.light?.abilitySO != null)
                    frameEvents += i.playerAbilitySet.light.FrameEvent;
                if (i.playerAbilitySet.heavy?.abilitySO != null)
                    frameEvents += i.playerAbilitySet.heavy.FrameEvent;
                if (i.playerAbilitySet.primary?.abilitySO != null)
                    frameEvents += i.playerAbilitySet.primary.FrameEvent;
            }
            currentAbilitySet = playerSetsList[0];
            currentAbilitySet.entityBody.modelPrefab.SetActive(true);
        }
        protected override void Update() {
            base.Update();
            InputPrimaryAttack();
            currentAbilitySet?.playerAbilitySet.heavy.Activate(currentAbilitySet.entityBody, GetInputValues.isPrimaryAbility);
        }
        public override void Die() {
            base.Die();
        }
        #endregion

        #region 
        public override void InputMove() {
            base.InputMove();
            currentAbilitySet?.playerAbilitySet.movement.Activate(currentAbilitySet.entityBody, true);
        }
        public override void InputPrimaryAttack() {
            base.InputPrimaryAttack();
            currentAbilitySet?.playerAbilitySet.light.Activate(currentAbilitySet.entityBody, GetInputValues.isPrimaryAttack);
        }
        #endregion

        #region Transitions
        public override void OnAbilityEvent(string eventMessage) {
            if (eventMessage == "") return;
            //Debug.Log(eventMessage);
            InputTransitionName(eventMessage);
        }
        public bool UnlockSet(string name) {
            if (playerSetsList.Any(x => x.abilitySetSO.abilitySetName == name)) {
                playerSetsList.First(x => x.abilitySetSO.abilitySetName == name).isUnlocked = true;
                return true;
            }
            return false;
        }
        #endregion

        #region Movement Functions
        public override void OnMoveEntity(Vector3 direction) {
            characterController.Move(direction);
            direction.y = 0;
            if (direction != Vector3.zero) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
        }
        public override void OnRotateEntity(Vector3 direction) {
            direction.y = 0;
            if (direction != Vector3.zero) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
        }
        #endregion

        #region Health
        public override void Damage(EntityDamage damage) {
        }
        public override void Heal(EntityDamage heal) {
        }
        #endregion

        #region Data
        public override EntityBody GetEntityBody() => currentAbilitySet.entityBody;

        [Serializable]
        public class PlayerSetSummary {
            public bool isUnlocked = false;
            public PlayerAbilitySetSO abilitySetSO;
            public EntityBody entityBody;
            [HideInInspector] public PlayerAbilitySet playerAbilitySet;
        }
        #endregion
    }
}