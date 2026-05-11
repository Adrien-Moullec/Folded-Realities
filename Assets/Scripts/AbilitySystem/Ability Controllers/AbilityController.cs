using System;
using System.Collections;

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

        protected virtual void Awake() {

        }
        protected virtual void Update() {
            frameEvents?.Invoke();
        }

        #region Movement Interface
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
            if (GetEntityBody().abilitySet?.healthSettings != null)
                GetEntityBody().abilitySet?.healthSettings.DamageAmount(GetEntityBody(), ref CurrentHealth, ref MaxHealth, damage);
            else
                HealthSO.DefaultDamage(GetEntityBody(), ref CurrentHealth, ref MaxHealth, damage);
            if (CurrentHealth <= 0) Die();
        }

        public virtual void Heal(EntityDamage heal) {
            if (GetEntityBody().abilitySet?.healthSettings != null)
                GetEntityBody().abilitySet?.healthSettings.HealAmount(GetEntityBody(), ref CurrentHealth, ref MaxHealth, heal);
            else
                HealthSO.DefaultHeal(GetEntityBody(), ref CurrentHealth, ref MaxHealth, heal);
        }
        public abstract void Die();
        protected IEnumerator PlayerDeath(AnimatorManager animatorManager, Action onDeathAnimationEnd) {

            bool hasFinishedAnim = false;
            yield return animatorManager?.InitiateOneOffAnimation(
                null,
                null,
                null,
                () => hasFinishedAnim = true,
                AnimationType.Death,
                true
            );
            while (!hasFinishedAnim)
                yield return null;

            onDeathAnimationEnd();
        }
        public virtual void InputTransitionName(string name) { }
        #endregion

        public abstract void OnDrawGizmos();
        public abstract bool IsGrounded();
    }
}