using UnityEngine;
using UnityEngine.UI;

namespace AbilitySystem {
    public class SingleAbilityEnemyController : SingleSetBaseEnemyController {
        [Header("Health bar")]
        [SerializeField] public Slider healthBar;
        /*
        private void Start() {
            healthBar.maxValue = MaxHealth;
            healthBar.value = MaxHealth;
        }*/
        public override void Damage(EntityDamage damage) {
            //base.Damage(damage);
            UpdateHealth();
        }
        public override void Heal(EntityDamage heal) {
            //base.Heal(heal);
            UpdateHealth();
        }
        private void UpdateHealth() {
            //healthBar.value = currentHealth;
        }
    }
}