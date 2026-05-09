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
        public GameObject BodyHolder;
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
                i.entityBody.animatorManager?.gameObject.SetActive(false);

                i.playerAbilitySet = new PlayerAbilitySet(i.abilitySetSO, i.entityBody);
                i.playerAbilitySet.movement.AbilityData = playerSetsList[0].playerAbilitySet.movement.AbilityData;

                if (i.playerAbilitySet.movement?.movementSO != null)
                    frameEvents += () => { i.playerAbilitySet.movement.FrameEvent(i.entityBody); };
                if (i.playerAbilitySet.primary?.abilitySO != null)
                    frameEvents += () => { i.playerAbilitySet.primary.FrameEvent(i.entityBody); };
                if (i.playerAbilitySet.secondary?.abilitySO != null)
                    frameEvents += () => { i.playerAbilitySet.secondary.FrameEvent(i.entityBody); };
                if (i.playerAbilitySet.tertiary?.abilitySO != null)
                    frameEvents += () => { i.playerAbilitySet.tertiary.FrameEvent(i.entityBody); };
            }
            SetNewSummary(playerSetsList[0]);
        }
        public override bool IsGrounded() =>
            characterController.isGrounded;

        protected override void Update() {
            base.Update();
            currentAbilitySet?.playerAbilitySet?.movement?.Activate(currentAbilitySet.entityBody, true);
            currentAbilitySet?.playerAbilitySet?.primary?.Activate(currentAbilitySet.entityBody, GetInputValues.isPrimaryAbility);
            currentAbilitySet?.playerAbilitySet?.secondary?.Activate(currentAbilitySet.entityBody, GetInputValues.isSecondaryAbility);
            currentAbilitySet?.playerAbilitySet?.tertiary?.Activate(currentAbilitySet.entityBody, GetInputValues.isTertiaryAbility);
        }
        public void QuickSwitch() {
            if (currentAbilitySet.abilitySetSO.abilitySetName == "Kuhaku") OnAbilityEvent("Scissors");
            else if (currentAbilitySet.abilitySetSO.abilitySetName == "Scissors") OnAbilityEvent("Kuhaku");
        }
        public override void Die() {
            base.Die();
            isDead = true;
            StartCoroutine(PlayerDeath());
        }
        IEnumerator PlayerDeath() {
            bool hasFinishedAnim = false;
            yield return currentAbilitySet.entityBody.animatorManager.InitiateOneOffAnimation(
                () => GetComponent<CharacterController>().enabled = false,
                null,
                null,
                () => hasFinishedAnim = true,
                AnimationType.Death,
                true
            );
            while (!hasFinishedAnim)
                yield return null;

            GetComponent<CharacterController>().enabled = true;
            CheckpointManager.Instance?.RespawnPlayer(gameObject);
            isDead = false;
            currentHealth = (int)MaxHealth;
        }
        #endregion

        #region Transitions
        public override void InputTransitionName(string name) {
            OnAbilityEvent(name);
            Debug.Log(name);
        }
        public override void OnAbilityEvent(string eventMessage) {
            if (!TryGetSetSummary(eventMessage, out PlayerSetSummary playerSetSummary) || playerSetSummary == currentAbilitySet)
                return;

            StartCoroutine(Transition(playerSetSummary));
        }
        private IEnumerator Transition(PlayerSetSummary newSummary) {
            if (!newSummary.isUnlocked) yield break;
            paperParticleDelta?.StartDelta();

            yield return currentAbilitySet.entityBody.animatorManager.InitiateOneOffAnimation(
                null,
                (f) => { paperParticleDelta?.UpdateDelta(f); },
                null,
                () => {
                },
                AnimationType.TransformOut,
                true
            );
            SetNewSummary(newSummary);

            currentAbilitySet.playerAbilitySet.movement.StartUp(currentAbilitySet.entityBody);

            yield return currentAbilitySet.entityBody.animatorManager.InitiateOneOffAnimation(
                null,
                (f) => { paperParticleDelta?.UpdateDelta(1 - f); },
                null,
                () => { paperParticleDelta?.EndDelta(); },
                AnimationType.TransformIn,
                true
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
        public override void OnMoveEntity(Vector3 direction, bool rotate = true) {
            characterController.Move(direction);
            direction.y = 0;
            if (direction != Vector3.zero && rotate) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
        }
        public override void OnRotateEntity(Vector3 direction) {
            direction.y = 0;
            if (direction != Vector3.zero) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
        }
        public override void OnEntityTrack(Vector3 location) {
            Debug.Log("TRACK");
        }
        #endregion

        #region Data
        public override EntityBody GetEntityBody() {
            if (currentAbilitySet != null) return currentAbilitySet.entityBody;
            else return playerSetsList[0].entityBody;
        }
        public override void OnDrawGizmos() {
            foreach (var n in playerSetsList) {
                n?.abilitySetSO?.movement?.GizmoEvent(n.entityBody);
                n?.abilitySetSO?.primary?.GizmoEvent(n.entityBody);
                n?.abilitySetSO?.secondary?.GizmoEvent(n.entityBody);
                n?.abilitySetSO?.tertiary?.GizmoEvent(n.entityBody);
            }
        }

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