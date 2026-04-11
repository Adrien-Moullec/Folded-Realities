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
        [SerializeField] private PaperParticles paperParticleDelta;
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

            currentAbilitySet = null;
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
                if (i.playerAbilitySet.primary?.abilitySO != null)
                    frameEvents += i.playerAbilitySet.primary.FrameEvent;
                if (i.playerAbilitySet.secondary?.abilitySO != null)
                    frameEvents += i.playerAbilitySet.secondary.FrameEvent;
                if (i.playerAbilitySet.primary?.abilitySO != null)
                    frameEvents += i.playerAbilitySet.primary.FrameEvent;
            }
            SetNewSummary(playerSetsList[0]);
        }
        void Start() {
            //OnAbilityEvent("Crane");
        }
        protected override void Update() {
            base.Update();
            currentAbilitySet?.playerAbilitySet?.movement?.Activate(currentAbilitySet.entityBody, true);
            currentAbilitySet?.playerAbilitySet?.primary?.Activate(currentAbilitySet.entityBody, GetInputValues.isPrimaryAttack);
            currentAbilitySet?.playerAbilitySet?.secondary?.Activate(currentAbilitySet.entityBody, GetInputValues.isSecondaryAttack);
        }
        public override void Die() {
            base.Die();
        }
        #endregion

        #region Transitions
        public override void OnAbilityEvent(string eventMessage) {
            if (!TryGetSetSummary(eventMessage, out PlayerSetSummary playerSetSummary) || playerSetSummary == currentAbilitySet) return;

            print("TURN INTO: " + eventMessage);
            StartCoroutine(Transition(playerSetSummary));
        }
        private IEnumerator Transition(PlayerSetSummary newSummary) {
            paperParticleDelta.StartDelta();
            yield return currentAbilitySet.entityBody.animatorManager.InitiateOneOffAnimation(
                null,
                (f) => { paperParticleDelta.UpdateDelta(f); },
                null,
                null,
                AnimationType.TransitionOut
            );
            SetNewSummary(newSummary);
            yield return currentAbilitySet.entityBody.animatorManager.InitiateOneOffAnimation(
                null,
                (f) => { paperParticleDelta.UpdateDelta(1 - f); },
                null,
                null,
                AnimationType.TransitionIn,
                false
            );
            paperParticleDelta.EndDelta();
        }
        private void SetNewSummary(PlayerSetSummary playerSetSummary) {
            currentAbilitySet?.entityBody.modelPrefab.SetActive(false);
            currentAbilitySet = playerSetSummary;
            currentAbilitySet?.entityBody.modelPrefab.SetActive(true);
        }
        public bool UnlockSet(string name) {
            if (TryGetSetSummary(name, out PlayerSetSummary playerSetSummary)) {
                playerSetSummary.isUnlocked = true;
                return true;
            }
            return false;
        }
        private bool TryGetSetSummary(string nameCheck, out PlayerSetSummary playerSetSummary) {
            if (playerSetsList.Any(x => x.abilitySetSO.abilitySetName == nameCheck)) {
                playerSetSummary = playerSetsList.First(x => x.abilitySetSO.abilitySetName == nameCheck);
                return true;
            }
            playerSetSummary = null;
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