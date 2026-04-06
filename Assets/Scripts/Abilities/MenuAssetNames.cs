using UnityEngine;

namespace AbilitySystem {
    public static class MenuAssetNames {
        #region Base Names
        public const string Root = "Origami";
        public const string Health = Root + "/Health";
        public const string AbilitySet = Root + "/Ability Set";
        #endregion

        #region Movement Names
        public const string MovementAbility = Root + "/Movement";
        #endregion

        #region Attacks
        public const string AttackAbility = Root + "/Attacks";
        #endregion

        #region Cooldown Names
        public const string CooldownAbility = Root + "/Cooldown";
        public const string Projectiles = CooldownAbility + "/Projectile";
        #endregion
    }
}