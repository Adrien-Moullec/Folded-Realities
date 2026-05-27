using System;
using System.Collections;
using System.Linq;

using UnityEngine;

namespace AbilitySystem {
    public abstract class AbilityController : MonoBehaviour, IAbility, IHealth {
        [Header("Team")]
        [Tooltip("The 'team' the entity is on.")]
        [SerializeField] public EntityTeam entityTeam;
        public EntityTeam GetEntityTeam => entityTeam;
        public int MaxHealth = 100;
        public int CurrentHealth;

        public AbilityControllerValues GetInputValues { get; set; } = new();
        protected delegate void FrameEvents();
        protected FrameEvents frameEvents;
        public AbilityController GetAbilityController { get => this; }
        public bool isControllerActive = true;
        float hitFrameTime = 1;

        [ContextMenu("Die")]
        public void ActivateDie() {
            Die();
        }


        protected virtual void Awake() {

        }
        protected virtual void Update() {
            if (!isControllerActive) return;
            frameEvents?.Invoke();
        }

        #region Movement Interface
        public virtual void OnEnable() {
            isControllerActive = true;
        }
        public virtual void OnDisable() {
            isControllerActive = false;
        }
        public abstract void OnRotateEntity(Vector3 movement);
        public abstract void OnMoveEntity(Vector3 direction, bool rotate = true);
        public abstract void OnEntityTrack(Vector3 location);
        #endregion

        #region Utility
        public abstract EntityBody GetEntityBody();
        public virtual void OnAbilityEvent(string eventMessage) { }
        #endregion

        #region Health
        public virtual void Damage(EntityDamage damage) {
            if (EntityTeamFunctions.HasCommonTeam(entityTeam, damage.damagingTeam)) return;
            if (GetEntityBody().abilitySet?.healthSettings != null)
                GetEntityBody().abilitySet?.healthSettings.DamageAmount(GetEntityBody(), ref CurrentHealth, ref MaxHealth, damage);
            else
                HealthSO.DefaultDamage(GetEntityBody(), ref CurrentHealth, ref MaxHealth, damage);
            if (CurrentHealth <= 0) Die();
            foreach (var n in GetEntityBody().entityShader.Select(x => x.material)) {
                n.SetFloat("_Health01", (float)CurrentHealth / MaxHealth);
            }
        }
        public virtual IEnumerator OnHitFrames() {
            float time = 0;
            while (time < hitFrameTime) {
                time += Time.deltaTime;
                foreach (var n in GetEntityBody().entityShader)
                    n.material.SetFloat("_DamageFlash01", Mathf.Abs(Mathf.Sin(time * 8 / hitFrameTime)));
                yield return null;
            }
            Debug.Log("END");
            foreach (var n in GetEntityBody().entityShader)
                n.material.SetFloat("_DamageFlash01", 0);
        }

        public virtual void Heal(EntityDamage heal) {
            if (GetEntityBody().abilitySet?.healthSettings != null)
                GetEntityBody().abilitySet?.healthSettings.HealAmount(GetEntityBody(), ref CurrentHealth, ref MaxHealth, heal);
            else
                HealthSO.DefaultHeal(GetEntityBody(), ref CurrentHealth, ref MaxHealth, heal);
            foreach (var n in GetEntityBody().entityShader.Select(x => x.material))
                n.SetFloat("_Health01", (float)CurrentHealth / MaxHealth);
        }
        public abstract void Die();
        public virtual void InputTransitionName(string name) { }
        #endregion

        public abstract void OnDrawGizmos();
        public abstract bool IsGrounded();

        public void SetMaxHealth() {
            CurrentHealth = MaxHealth;
        }
    }
}