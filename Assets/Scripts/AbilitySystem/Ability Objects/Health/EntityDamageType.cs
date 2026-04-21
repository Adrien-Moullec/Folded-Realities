using System;

namespace AbilitySystem {
    [Flags]
    public enum EntityDamageType {
        Normal = 0,
        Melee = 1 << 0,
        Fire = 1 << 1,
        Ice = 1 << 2,
        Water = 1 << 3,
    }
}