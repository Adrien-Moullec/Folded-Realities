using UnityEngine;

using System.Collections;
using System.Linq;
using System.Collections.Generic;

using Unity.VisualScripting;

namespace AbilitySystem {
    [RequireComponent(typeof(AnimatorManager))]
    public abstract class AbilityController : MonoBehaviour, IAbility, IHealth {
        [Header("Team")]
        [Tooltip("The 'team' the entity is on.")]
        [SerializeField] EntityTeam entityTeam;
        public EntityTeam GetEntityTeam => entityTeam;

        [Header("Health")]
        [Tooltip("Max health.")]
        [SerializeField, Min(1)] int maxHealth = 100;
        [Tooltip("Current entity health.")]
        [HideInInspector] protected int currentHealth = 100;
        [Tooltip("Current entity health.")]
        [HideInInspector] protected bool canUseAbilities = true;
        public AbilityInputValues GetInputValues { get; set; } = new();
        protected delegate void FrameEvents();
        protected FrameEvents frameEvents;
        public float CurrentHealth => throw new System.NotImplementedException();
        public float MaxHealth => throw new System.NotImplementedException();

        public AbilityController GetAbilityController { get => this; }
        private AnimatorManager animatorManager;

        protected virtual void Awake() {
            animatorManager = GetComponent<AnimatorManager>();
            currentHealth = maxHealth;
        }
        protected virtual void Update() {
            frameEvents?.Invoke();
            InputMove();
        }
        #region Input Interface
        public virtual void InputTransitionName(string back) { }
        public virtual void InputMove() {

        }
        public virtual void InputPrimaryAttack() {
            if (!canUseAbilities) return;
        }
        public virtual void InputPrimaryAbility() {
            if (!canUseAbilities) return;
        }
        #endregion

        #region Movement Interface
        public abstract void OnRotateEntity(Vector3 movement);
        public abstract void OnMoveEntity(Vector3 direction);
        #endregion

        #region Utility
        public abstract EntityBody GetEntityBody();
        public virtual void OnAbilityEvent(string eventMessage) { }
        #endregion

        #region Health
        public virtual void Damage(EntityDamage damage) {
            if (!EntityTeamFunctions.HasCommonTeam(GetEntityTeam, damage.team))
                currentHealth -= (int)damage.amount;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth <= 0)
                Die();
        }

        public virtual void Heal(EntityDamage heal) {
            currentHealth += (int)heal.amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        public virtual void Die() {

        }

        #endregion
    }
}