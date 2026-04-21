using UnityEngine;

namespace AbilitySystem {
    public abstract class AbilityController : MonoBehaviour, IAbility, IHealth {
        [Header("Team")]
        [Tooltip("The 'team' the entity is on.")]
        [SerializeField] public EntityTeam entityTeam;
        public EntityTeam GetEntityTeam => entityTeam;

        [Header("Health")]
        [Tooltip("Max health.")]
        [SerializeField, Min(1)] int maxHealth = 100;
        [Tooltip("Current entity health.")]
        [HideInInspector] protected int currentHealth = 100;
        [HideInInspector] protected bool isDead = false;
        public AbilityControllerValues GetInputValues { get; set; } = new();
        protected delegate void FrameEvents();
        protected FrameEvents frameEvents;
        public float CurrentHealth => throw new System.NotImplementedException();
        public float MaxHealth => throw new System.NotImplementedException();

        public AbilityController GetAbilityController { get => this; }

        protected virtual void Awake() {
            currentHealth = maxHealth;
        }
        protected virtual void Update() {
            frameEvents?.Invoke();
        }

        #region Movement Interface
        public abstract void OnRotateEntity(Vector3 movement);
        public abstract void OnMoveEntity(Vector3 direction);
        public abstract void OnEntityTrack(Vector3 location);
        #endregion

        #region Utility
        public abstract EntityBody GetEntityBody();
        public virtual void OnAbilityEvent(string eventMessage) { }
        #endregion

        #region Health
        public virtual void Damage(EntityDamage damage) {
            if (isDead) return;
            if (!EntityTeamFunctions.HasCommonTeam(GetEntityTeam, damage.damagingTeam))
                currentHealth -= (int)damage.amount;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth <= 0)
                Die();

            //Debug.Log(currentHealth);
        }

        public virtual void Heal(EntityDamage heal) {
            if (isDead) return;
            currentHealth += (int)heal.amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        public virtual void Die() {

        }

        public virtual void InputTransitionName(string name) {

        }

        #endregion

        public abstract void OnDrawGizmos();
    }
}