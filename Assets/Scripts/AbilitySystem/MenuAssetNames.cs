using UnityEngine;

namespace AbilitySystem {
    /// <summary>
    /// String references to building a menu for all ability system scritpable object creations for future ease of editability.
    /// </summary>
    public static class MenuAssetNames {
        public const string Root = "Origami";
        public const string Health = Root + "/Health";
        public const string AbilitySet = Root + "/Ability Set";
        public const string MovementAbility = Root + "/Movement Ability";
        public const string AttackAbility = Root + "/Attacks";
        public const string CooldownAbility = Root + "/Cooldown Ability";
        public const string Projectiles = CooldownAbility + "/Projectiles";
    }
}