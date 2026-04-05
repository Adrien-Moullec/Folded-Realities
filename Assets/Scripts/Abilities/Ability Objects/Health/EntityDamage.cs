using UnityEngine;

namespace AbilitySystem {
    public struct EntityDamage {
        public float amount;
        public EntityBody dealer;
        public EntityTeam team;
        public EntityDamageType type;
        public EntityDamage(float amount, EntityBody dealer) {
            this.amount = amount;
            this.dealer = dealer;
            team = EntityTeam.None;
            type = EntityDamageType.Normal;
        }
        public EntityDamage(float amount, EntityBody dealer, EntityTeam team) {
            this.amount = amount;
            this.dealer = dealer;
            this.team = team;
            type = EntityDamageType.Normal;
        }
        public EntityDamage(float amount, EntityBody dealer, EntityTeam team, EntityDamageType type) {
            this.amount = amount;
            this.dealer = dealer;
            this.team = team;
            this.type = type;
        }

    }
}