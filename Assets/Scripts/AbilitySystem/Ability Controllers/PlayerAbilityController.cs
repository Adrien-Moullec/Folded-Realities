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
        [SerializeField] PlayerHealthCanvas playerHealthCanvas;

        [Space]
        [Header("Respawn")]
        [SerializeField] Transform startPoint;
        [Header("Damage Settings")]
        [SerializeField] float invincibilityTime = 1f;
        private bool invincible = false;

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
            CurrentHealth = MaxHealth;
        }
        public override void OnEnable() {
            GetComponent<PlayerManager>().OnEnable();
        }
        public override void OnDisable() {
            GetComponent<PlayerManager>().OnDisable();
        }
        public override bool IsGrounded() =>
            characterController.isGrounded;

        protected override void Update() {
            Debug.Log(characterController.isGrounded);
            if (!characterController.enabled) return;
            base.Update();
            currentAbilitySet?.playerAbilitySet?.movement?.Activate(currentAbilitySet.entityBody, true);
            currentAbilitySet?.playerAbilitySet?.primary?.Activate(currentAbilitySet.entityBody, GetInputValues.isPrimaryAbility);
            currentAbilitySet?.playerAbilitySet?.secondary?.Activate(currentAbilitySet.entityBody, GetInputValues.isSecondaryAbility);
            currentAbilitySet?.playerAbilitySet?.tertiary?.Activate(currentAbilitySet.entityBody, GetInputValues.isTertiaryAbility);
        }
        public void QuickSwitch() {
            if (currentAbilitySet.abilitySetSO?.abilitySetName == "Kuhaku") OnAbilityEvent("Bear");
            else if (currentAbilitySet.abilitySetSO?.abilitySetName == "Bear") OnAbilityEvent("Kuhaku");
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
        // Andrea's function
        IEnumerator InvincibilityFrames() {
            invincible = true;
            float time = 0;
            while (time < invincibilityTime) {
                time += Time.deltaTime;
                foreach (var n in GetEntityBody().entityShader)
                    n.material.SetFloat("_DamageFlash01", Mathf.Abs(Mathf.Sin(time * 8 / invincibilityTime)));
                yield return null;
            }
            Debug.Log("END");
            foreach (var n in GetEntityBody().entityShader)
                n.material.SetFloat("_DamageFlash01", 0);
            invincible = false;
        }
        // Andrea's function
        void Respawn() {

            playerHealthCanvas.UpdateHearts(
                100
            );

            CurrentHealth =
                MaxHealth;
        }
        //andrea updated for checkpoint to restore health
        public void RestoreFullHealth() {

            CurrentHealth =
                MaxHealth;

            playerHealthCanvas?.UpdateHearts(
                CurrentHealth
            );
        }

        public void DirectDamage(
            int amount
        ) {

            CurrentHealth -=
                amount;

            if (
                CurrentHealth < 0
            ) {
                CurrentHealth = 0;
            }

            playerHealthCanvas?.UpdateHearts(
                CurrentHealth
            );

            if (
                CurrentHealth <= 0
            ) {
                Die();
            }
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
        public override void Die() {

            currentAbilitySet
                .abilitySetSO
                .healthSettings
                ?.Die(
                    currentAbilitySet.entityBody,
                    ref CurrentHealth
                );

            GetComponent<
                CharacterController
            >().enabled = false;

            StartCoroutine(
                PlayerDeath(
                    currentAbilitySet.entityBody.animatorManager,
                    () => {

                        StartCoroutine(
                            SceneTransition.Instance
                                .RespawnTransition(
                                    gameObject
                                )
                        );

                        GetComponent<
                            CharacterController
                        >().enabled = true;

                        CheckpointManager.Instance
                            ?.RespawnPlayer(
                                gameObject
                            );

                        currentAbilitySet
                            .abilitySetSO
                            .healthSettings
                            .MaxHealth(
                                currentAbilitySet.entityBody,
                                ref CurrentHealth,
                                ref MaxHealth
                            );

                        Respawn();
                    }
                )
            );
        }
        public override void Heal(EntityDamage heal) {
            base.Heal(heal);
            playerHealthCanvas?.UpdateHearts(CurrentHealth);
        }
        public override void Damage(EntityDamage damage) {
            if (invincible) return;
            base.Damage(damage);
            playerHealthCanvas?.UpdateHearts(CurrentHealth);
            StartCoroutine(InvincibilityFrames());
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