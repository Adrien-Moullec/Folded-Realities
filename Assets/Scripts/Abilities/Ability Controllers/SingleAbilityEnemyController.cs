using UnityEngine;
using UnityEngine.UI;

namespace AbilitySystem {
    public class SingleAbilityEnemyController : SingleSetBaseEnemyController {
        [Header("Health bar")]
        [SerializeField] public Slider healthBar;
        private void Start() {
            healthBar.maxValue = MaxHealth;
            healthBar.value = MaxHealth;
        }
        public override void Damage(EntityDamage damage) {
            base.Damage(damage);
            UpdateHealth();
        }

        public override void Die() {
            Destroy(gameObject);
        }

        public override void Heal(EntityDamage heal) {
            currentHealth = (int)Mathf.Clamp(currentHealth + (int)heal.amount, 0, MaxHealth);
            UpdateHealth();
        }

        private void UpdateHealth() {
            healthBar.value = currentHealth;
        }
    }
}