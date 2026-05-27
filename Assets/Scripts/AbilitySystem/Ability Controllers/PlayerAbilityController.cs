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

        [Space]
        [Header("Transition Animation")]
        [SerializeField] private PaperParticles paperParticleDelta;
        [Header("Boss Fight")]
        [SerializeField] bool reloadSceneOnDeath = false;

        [Space]
        [Header("Abilities")]
        [SerializeField] List<PlayerSetSummary> playerSetsList;
        [HideInInspector] public PlayerSetSummary currentAbilitySet;
        public GameObject BodyHolder;
        [HideInInspector] public CharacterController characterController;
        [SerializeField] PlayerHealthCanvas playerHealthCanvas;

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
                AnimationType.TransformOut.ToString(),
                true
            );
            SetNewSummary(newSummary);

            currentAbilitySet.playerAbilitySet.movement.StartUp(currentAbilitySet.entityBody);

            yield return currentAbilitySet.entityBody.animatorManager.InitiateOneOffAnimation(
                null,
                (f) => { paperParticleDelta?.UpdateDelta(1 - f); },
                null,
                () => { paperParticleDelta?.EndDelta(); },
                AnimationType.TransformIn.ToString(),
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
        public override IEnumerator OnHitFrames() {
            invincible = true;
            yield return base.OnHitFrames();
            invincible = false;
        }
        public override void OnRotateEntity(Vector3 direction) {
            if (direction != Vector3.zero) currentAbilitySet.entityBody.bodyHolder.transform.forward = direction;
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
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
        //andrea updated death
        public override void Die() {
            StartCoroutine(OnDie());
        }
        IEnumerator OnDie() {

            if (TryGetSetSummary("Kuhaku", out PlayerSetSummary playerSetSummary) && currentAbilitySet?.abilitySetSO.abilitySetName != "Kuhaku")
                yield return Transition(playerSetSummary);

            currentAbilitySet.abilitySetSO.healthSettings?.Die(currentAbilitySet.entityBody, ref CurrentHealth);

            GetComponent<CharacterController>().enabled = false;
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
        public override void Heal(EntityDamage heal) {
            base.Heal(heal);
            foreach (var n in GetEntityBody().entityShader)
                n.material.SetFloat("_Health01", CurrentHealth / MaxHealth);
            playerHealthCanvas?.UpdateHearts(CurrentHealth);
        }
        public override void Damage(EntityDamage damage) {
            if (EntityTeamFunctions.HasCommonTeam(damage.damagingTeam, entityTeam)) return;
            if (invincible)
                return;

            base.Damage(damage);
            playerHealthCanvas?.UpdateHearts(CurrentHealth);

            foreach (var n in GetEntityBody().entityShader)
                n.material.SetFloat("_Health01", CurrentHealth / MaxHealth);

            StartCoroutine(OnHitFrames());
            if (CurrentHealth <= 0)
                Die();
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
#if UNITY_EDITOR
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