using UnityEngine;
using UnityEngine.UI;

namespace AbilitySystem {
    public class SingleAbilityEnemyController : SingleSetEnemyController {
        [Header("HEALTHBAR")]
        [SerializeField] public UnityEngine.UI.Slider healthBar;
        private void Start() {
            Debug.Log(healthBar + ", " + healthBar.name);
            //base.Awake();
            healthBar.maxValue = MaxHealth;
            Debug.Log("TEST2");
            healthBar.value = MaxHealth;
            Debug.Log("TEST3");
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