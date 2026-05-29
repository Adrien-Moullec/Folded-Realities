using UnityEngine;

namespace AbilitySystem {
    /// <summary>
    /// Entity Damage holds information about when damage or health is applied from one entity to another
    /// </summary>
    public struct EntityDamage {
        [Tooltip("Damage/Health amount.")]
        public float amount;
        [Tooltip("The dealer of the damage.")]
        public EntityBody dealer;
        [Tooltip("The team that dealt the damage.")]
        public EntityTeam damagingTeam;
        [Tooltip("The type of damage inflicted.")]
        public EntityDamageType type;

        #region Setup Entity damage
        public EntityDamage(float amount, EntityBody dealer) {
            this.amount = amount;
            this.dealer = dealer;
            damagingTeam = EntityTeam.None;
            type = EntityDamageType.Normal;
        }
        public EntityDamage(float amount, EntityBody dealer, EntityTeam team) {
            this.amount = amount;
            this.dealer = dealer;
            this.damagingTeam = team;
            type = EntityDamageType.Normal;
        }
        public EntityDamage(float amount, EntityBody dealer, EntityTeam team, EntityDamageType type) {
            this.amount = amount;
            this.dealer = dealer;
            this.damagingTeam = team;
            this.type = type;
        }
        #endregion
    }
}