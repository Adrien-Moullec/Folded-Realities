using UnityEngine;
using UnityEngine.UI;

namespace AbilitySystem {
    public class SingleAbilityEnemyController : SingleSetEnemyController {
        [Header("HEALTHBAR")]
        [SerializeField] public Slider healthBar;
        protected override void Awake() {
            base.Awake();
            healthBar.maxValue = (float)MaxHealth;
            healthBar.value = (float)MaxHealth;
        }
        public override void Damage(float amount) {
            currentHealth -= (int)amount;
            if (currentHealth <= 0)
                Die();
            UpdateHealth();
        }

        public override void Die() {
            Destroy(gameObject);
        }

        public override void Heal(float amount) {
            currentHealth = (int)Mathf.Clamp(currentHealth + (int)amount, 0, MaxHealth);
            UpdateHealth();
        }

        private void UpdateHealth() {
            healthBar.value = currentHealth;
        }
    }
}