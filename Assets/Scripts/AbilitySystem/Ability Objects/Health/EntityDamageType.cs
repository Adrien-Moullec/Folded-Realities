using System;

namespace AbilitySystem {
    /// <summary>
    /// Entity damage type for damage between entities, allows for different types of afflictions
    /// </summary>
    [Flags]
    public enum EntityDamageType {
        Normal = 0,
        Melee = 1 << 0,
        Fire = 1 << 1,
        Ice = 1 << 2,
        Water = 1 << 3,
        Heavy = 1 << 4,
    }
}