using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using AbilitySystem;

namespace AbilitySystem {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerAbilityController : AbilityController {

        [Header("Transition Animation")]
        [Tooltip("Paper particles vfx for cooler transition between entities.")]
        [SerializeField] private PaperParticles paperParticleDelta;

        [Space]
        [Header("Abilities")]
        [Tooltip("List of player set abilities for each model.")]
        [SerializeField] List<PlayerSetSummary> playerSetsList;
        [Tooltip("Current ability set being played.")]
        [HideInInspector] public PlayerSetSummary currentAbilitySet;
        [Tooltip("Character controller for the entity.")]
        [HideInInspector] public CharacterController characterController;
        [Tooltip("Player canvas reference.")]
        [SerializeField] PlayerHealthCanvas playerHealthCanvas;

        [Header("Damage Settings")]
        [Tooltip("Invincibility time after being hit.")]
        [SerializeField] float invincibilityTime = 1f;
        [Tooltip("Invincibility status of player.")]
        private bool invincible = false;

        #region OnStart
        /// <summary>
        /// Setup player abilities, health and controller
        /// </summary>
        protected override void Awake() {
            base.Awake();

            currentAbilitySet = null;
            characterController = GetComponent<CharacterController>();

            /// Loop through each player set
            foreach (var i in playerSetsList) {
                if (i.abilitySetSO == null)
                    continue;

                /// Setup managers and interfaces
                i.entityBody.iAbility = this;
                i.entityBody.iHealth = this;
                i.entityBody.animatorManager?.gameObject.SetActive(false);

                /// Setup ability sets
                i.playerAbilitySet = new PlayerAbilitySet(i.abilitySetSO, i.entityBody);
                i.playerAbilitySet.movement.AbilityData = playerSetsList[0].playerAbilitySet.movement.AbilityData;

                /// Setup frame logic
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

        /// <summary>
        /// Enable player controls
        /// </summary>
        public override void OnEnable() {
            GetComponent<PlayerManager>().OnEnable();
        }
        /// <summary>
        /// Disable player controls
        /// </summary>
        public override void OnDisable() {
            GetComponent<PlayerManager>().OnDisable();
        }
        /// <summary>
        /// Gets the status of player ground state for Entity body
        /// </summary>
        public override bool IsGrounded() =>
            characterController.isGrounded;

        /// <summary>
        /// Continuously activate abilities based on player input
        /// </summary>
        protected override void Update() {
            if (!characterController.enabled) return;
            base.Update();
            currentAbilitySet?.playerAbilitySet?.movement?.Activate(currentAbilitySet.entityBody, true);
            currentAbilitySet?.playerAbilitySet?.primary?.Activate(currentAbilitySet.entityBody, GetInputValues.isPrimaryAbility);
            currentAbilitySet?.playerAbilitySet?.secondary?.Activate(currentAbilitySet.entityBody, GetInputValues.isSecondaryAbility);
            currentAbilitySet?.playerAbilitySet?.tertiary?.Activate(currentAbilitySet.entityBody, GetInputValues.isTertiaryAbility);
        }
        /// <summary>
        /// Quickly switch between the bear and Kuhaku
        /// </summary>
        public void BearSwitch() {
            if (currentAbilitySet.abilitySetSO?.abilitySetName == "Kuhaku") OnAbilityEvent("Bear");
            else if (currentAbilitySet.abilitySetSO?.abilitySetName == "Bear") OnAbilityEvent("Kuhaku");
        }
        #endregion

        #region Transitions
        /// <summary>
        /// Search for ability set to transition to
        /// </summary>
        /// <param name="name"> target transition goal </param>
        public override void InputTransitionName(string name) {
            OnAbilityEvent(name);
        }
        /// <summary>
        /// Custom events for player to transition under conditions
        /// </summary>
        /// <param name="eventMessage"> event message </param>
        public override void OnAbilityEvent(string eventMessage) {
            if (!TryGetSetSummary(eventMessage, out PlayerSetSummary playerSetSummary) || playerSetSummary == currentAbilitySet)
                return;

            StartCoroutine(Transition(playerSetSummary));
        }

        /// <summary>
        /// Transition to next avatar
        /// </summary>
        /// <param name="newSummary"> player set of new model </param>
        /// <returns></returns>
        private IEnumerator Transition(PlayerSetSummary newSummary) {

            /// Only transition if unlocked
            if (!newSummary.isUnlocked) yield break;
            paperParticleDelta?.StartDelta();

            /// Call transform animation
            yield return currentAbilitySet.entityBody.animatorManager.InitiateOneOffAnimation(
                null,
                (f) => { paperParticleDelta?.UpdateDelta(f); },
                null,
                () => {
                },
                AnimationType.TransformOut.ToString(),
                true
            );
            SetNewSummary(newSummary);

            /// Start movement logic
            currentAbilitySet.playerAbilitySet.movement.StartUp(currentAbilitySet.entityBody);

            /// Transform in model animation
            yield return currentAbilitySet.entityBody.animatorManager.InitiateOneOffAnimation(
                null,
                (f) => { paperParticleDelta?.UpdateDelta(1 - f); },
                null,
                () => { paperParticleDelta?.EndDelta(); },
                AnimationType.TransformIn.ToString(),
                true
            );
        }

        /// <summary>
        /// Set new player ability set
        /// </summary>
        /// <param name="playerSetSummary"> set to switch to </param>
        private void SetNewSummary(PlayerSetSummary playerSetSummary) {
            currentAbilitySet?.entityBody.animatorManager.gameObject.SetActive(false);
            currentAbilitySet = playerSetSummary;
            currentAbilitySet?.entityBody.animatorManager.gameObject.SetActive(true);
        }

        /// <summary>
        /// Unlock set by name so it is available to transform into
        /// </summary>
        /// <param name="name"> Ability set name </param>
        /// <returns> true on success </returns>
        public bool UnlockSet(string name) {
            if (TryGetSetSummary(name, out PlayerSetSummary playerSetSummary)) {
                playerSetSummary.isUnlocked = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Try get player set summary by name
        /// </summary>
        /// <param name="nameCheck"> Name to check for </param>
        /// <param name="playerSetSummary"> receive new player set summary if available </param>
        /// <returns> true on ability set found </returns>
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

        /// <summary>
        /// Move entity in direction
        /// </summary>
        /// <param name="direction"> directional movement </param>
        /// <param name="rotate"> optional rotate towards direction </param>
        public override void OnMoveEntity(Vector3 direction, bool rotate = true) {
            characterController.Move(direction);
            direction.y = 0;
            if (direction != Vector3.zero && rotate) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
        }
        // Andrea's function
        public override IEnumerator OnHitFrames() {
            invincible = true;
            yield return base.OnHitFrames();
            invincible = false;
        }

        /// <summary>
        /// Rotate entity body towards a direction
        /// </summary>
        public override void OnRotateEntity(Vector3 direction) {
            if (direction != Vector3.zero) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        }

        /// <summary>
        /// Set a location for the entity to move towards
        /// </summary>
        public override void OnEntityTrack(Vector3 location) {
            Debug.Log("TRACK");
        }
        #endregion

        #region Data
        /// <summary>
        /// Get entity body information
        /// </summary>
        public override EntityBody GetEntityBody() {
            if (currentAbilitySet != null) return currentAbilitySet.entityBody;
            else return playerSetsList[0].entityBody;
        }

        /// <summary>
        /// Draw ability gizmos
        /// </summary>
        public override void OnDrawGizmos() {
            foreach (var n in playerSetsList) {
                n?.abilitySetSO?.movement?.GizmoEvent(n.entityBody);
                n?.abilitySetSO?.primary?.GizmoEvent(n.entityBody);
                n?.abilitySetSO?.secondary?.GizmoEvent(n.entityBody);
                n?.abilitySetSO?.tertiary?.GizmoEvent(n.entityBody);
            }
        }
        //andrea updated death
        public override void Die() {
            StartCoroutine(OnDie());
        }

        /// <summary>
        /// On player die, call animation and start respawn transition
        /// </summary>
        IEnumerator OnDie() {

            /// Transform back to kuhaku for death
            if (TryGetSetSummary("Kuhaku", out PlayerSetSummary playerSetSummary) && currentAbilitySet?.abilitySetSO.abilitySetName != "Kuhaku")
                yield return Transition(playerSetSummary);
            currentAbilitySet.abilitySetSO.healthSettings?.Die(currentAbilitySet.entityBody, ref CurrentHealth);

            GetComponent<CharacterController>().enabled = false;

            /// Transition after Kuhaku death
            bool isFin = false;
            yield return GetEntityBody().animatorManager.InitiateOneOffAnimation(
                null,
                (x) => {
                    foreach (var n in GetEntityBody().entityShader)
                        n.material.SetFloat("_DissolveValue", x);
                    Debug.Log(x);
                },
                null,
                () => isFin = true,
                AnimationType.Death.ToString(),
                true,
                0.2f
            );
            while (!isFin) yield return null;

            /// Respawn Kuhaku to last save location and reset health
            StartCoroutine(GameplaySystem.instance.Respawn());
            GetComponent<CharacterController>().enabled = true;
            currentAbilitySet?
                .abilitySetSO?
                .healthSettings
                ?.MaxHealth(
                    currentAbilitySet.entityBody,
                    ref CurrentHealth,
                    ref MaxHealth
                );

            SetMaxHealth();
            foreach (var n in GetEntityBody().entityShader)
                n.material.SetFloat("_DissolveValue", 0);
        }

        /// <summary>
        /// Set max health
        /// </summary>
        public override void SetMaxHealth() {
            base.SetMaxHealth();
            playerHealthCanvas?.UpdateHearts(CurrentHealth);
        }

        /// <summary>
        /// Heal player by an amount
        /// </summary>
        public override void Heal(EntityDamage heal) {
            base.Heal(heal);
            foreach (var n in GetEntityBody().entityShader)
                n.material.SetFloat("_Health01", CurrentHealth / MaxHealth);
            playerHealthCanvas?.UpdateHearts(CurrentHealth);
        }

        /// <summary>
        /// Damage entity by an amount
        /// </summary>
        public override void Damage(EntityDamage damage) {
            /// check to see if damage is from an enemy
            if (EntityTeamFunctions.HasCommonTeam(damage.damagingTeam, entityTeam)) return;
            if (invincible)
                return;

            /// Do damage that affects shader
            base.Damage(damage);
            playerHealthCanvas?.UpdateHearts(CurrentHealth);
            foreach (var n in GetEntityBody().entityShader)
                n.material.SetFloat("_Health01", CurrentHealth / MaxHealth);
            StartCoroutine(OnHitFrames());

            /// Check for death
            if (CurrentHealth <= 0)
                Die();
        }

        /// <summary>
        /// Set summary that contains ability and entity information
        /// </summary>
        [Serializable]
        public class PlayerSetSummary {
            [Tooltip("Is set unlocked or not.")]
            public bool isUnlocked = false;
            [Tooltip("Ability set information for a model.")]
            public PlayerAbilitySetSO abilitySetSO;
            [Tooltip("Entity body information.")]
            public EntityBody entityBody;
            [Tooltip("Player ability set.")]
            [HideInInspector] public PlayerAbilitySet playerAbilitySet;
        }
        #endregion
    }
}
#if UNITY_EDITOR
/// <summary>
/// Editor window for player to test death
/// </summary>
[CustomEditor(typeof(PlayerAbilityController))]
[CanEditMultipleObjects]
public class PlayerAbilityControllerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("Die")) {
            PlayerAbilityController s = (PlayerAbilityController)target;
            s.Die();
        }
        if (GUILayout.Button("Hurt")) {
            PlayerAbilityController s = (PlayerAbilityController)target;
            s.StartCoroutine(s.OnHitFrames());
        }
    }
}
#endif