using UnityEngine;
using UnityEngine.UI;

namespace AbilitySystem {
    /// <summary>
    /// Enemy ability controller, an older script designed to work with world canvas
    /// </summary>
    public class SingleAbilityEnemyController : SingleSetBaseEnemyController {

        [Header("Health bar")]
        [Tooltip("Health bar reference.")]
        [SerializeField] public Slider healthBar;

        /// <summary>
        /// On damage, update canvas
        /// </summary>
        public override void Damage(EntityDamage damage) {
            base.Damage(damage);
            UpdateHealth();
        }

        /// <summary>
        /// On heal, 
        /// </summary>
        public override void Heal(EntityDamage heal) {
            base.Heal(heal);
            UpdateHealth();
        }

        /// <summary>
        /// Old function to update health
        /// </summary>
        private void UpdateHealth() {
        }
    }
}