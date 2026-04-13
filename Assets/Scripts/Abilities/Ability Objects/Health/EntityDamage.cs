using UnityEngine;

namespace AbilitySystem {
    public struct EntityDamage {
        public float amount;
        public EntityBody dealer;
        public EntityTeam damagingTeam;
        public EntityDamageType type;
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

    }
}