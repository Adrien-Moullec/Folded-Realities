using UnityEngine;
using UnityEngine.UI;

namespace AbilitySystem {
    public class SingleAbilityEnemyController : SingleSetEnemyController {
        [Header("Health bar")]
        [SerializeField] public Slider healthBar;
        private void Start() {
            healthBar.maxValue = MaxHealth;
            healthBar.value = MaxHealth;
        }
        public override void Damage(float amount, EntityBody otherBody = null) {
            base.Damage(amount);
            UpdateHealth();
        }

        public override void Die() {
            Destroy(gameObject);
        }

        public override void Heal(float amount, EntityBody otherBody = null) {
            currentHealth = (int)Mathf.Clamp(currentHealth + (int)amount, 0, MaxHealth);
            UpdateHealth();
        }

        private void UpdateHealth() {
            healthBar.value = currentHealth;
        }
    }
}