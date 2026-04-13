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
                i.entityBody.iHealth = this;
                i.entityBody.animatorManager.gameObject.SetActive(false);

                i.playerAbilitySet = new PlayerAbilitySet(i.abilitySetSO, i.entityBody);
                i.playerAbilitySet.movement.AbilityData = playerSetsList[0].playerAbilitySet.movement.AbilityData;

                if (i.playerAbilitySet.movement?.movementSO != null)
                    frameEvents += i.playerAbilitySet.movement.FrameEvent;
                if (i.playerAbilitySet.primary?.abilitySO != null)
                    frameEvents += i.playerAbilitySet.primary.FrameEvent;
                if (i.playerAbilitySet.secondary?.abilitySO != null)
                    frameEvents += i.playerAbilitySet.secondary.FrameEvent;
                if (i.playerAbilitySet.tertiary?.abilitySO != null)
                    frameEvents += i.playerAbilitySet.tertiary.FrameEvent;
            }
            SetNewSummary(playerSetsList[0]);
        }
        protected override void Update() {
            base.Update();
            currentAbilitySet?.playerAbilitySet?.movement?.Activate(currentAbilitySet.entityBody, true);
            currentAbilitySet?.playerAbilitySet?.primary?.Activate(currentAbilitySet.entityBody, GetInputValues.isPrimaryAbility);
            currentAbilitySet?.playerAbilitySet?.secondary?.Activate(currentAbilitySet.entityBody, GetInputValues.isSecondaryAbility);
            currentAbilitySet?.playerAbilitySet?.tertiary?.Activate(currentAbilitySet.entityBody, GetInputValues.isTertiaryAbility);
            //Debug.Log()
        }
        public override void Die() {
            base.Die();
        }
        #endregion

        #region Transitions
        public override void OnAbilityEvent(string eventMessage) {
            if (!TryGetSetSummary(eventMessage, out PlayerSetSummary playerSetSummary) || playerSetSummary == currentAbilitySet)
                return;
            StartCoroutine(Transition(playerSetSummary));
        }
        private IEnumerator Transition(PlayerSetSummary newSummary) {
            paperParticleDelta.StartDelta();
            yield return currentAbilitySet.entityBody.animatorManager.InitiateOneOffAnimation(
                () => { Debug.Log(currentAbilitySet.abilitySetSO.abilitySetName + ": Start"); },
                (f) => { paperParticleDelta.UpdateDelta(f); },
                null,
                () => {
                },
                AnimationType.TransformOut,
                true
            );
            SetNewSummary(newSummary);
            Debug.Log(currentAbilitySet.abilitySetSO.abilitySetName + ": new summary");
            currentAbilitySet.playerAbilitySet.movement.StartUp(currentAbilitySet.entityBody);

            yield return currentAbilitySet.entityBody.animatorManager.InitiateOneOffAnimation(
                () => { Debug.Log(currentAbilitySet.abilitySetSO.abilitySetName + ": new summary start"); },
                (f) => { paperParticleDelta.UpdateDelta(1 - f); },
                null,
                () => { paperParticleDelta.EndDelta(); Debug.Log(currentAbilitySet.abilitySetSO.abilitySetName + ": new summary end"); },
                AnimationType.TransformIn,
                false
            );
        }
        private void SetNewSummary(PlayerSetSummary playerSetSummary) {
            currentAbilitySet?.entityBody.animatorManager.gameObject.SetActive(false);
            currentAbilitySet = playerSetSummary;
            currentAbilitySet?.entityBody.animatorManager.gameObject.SetActive(true);
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
        public override void OnEntityTrack(Vector3 location) {
            Debug.Log("TRACK");
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