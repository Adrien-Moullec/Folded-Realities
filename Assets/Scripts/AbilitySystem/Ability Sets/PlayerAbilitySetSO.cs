using System;

using UnityEngine;

namespace AbilitySystem {

    /// <summary>
    /// Player ability set scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "Ability Set", menuName = MenuAssetNames.AbilitySet + "/Player Ability Set", order = 0)]
    public class PlayerAbilitySetSO : AbilitySetSO {
        [Tooltip("Primary cooldown ability.")]
        [SerializeField] public CooldownAbilitySO primary;
        [Tooltip("Secondary cooldown ability.")]
        [SerializeField] public CooldownAbilitySO secondary;
        [Tooltip("Tertiary cooldown ability.")]
        [SerializeField] public CooldownAbilitySO tertiary;
    }

    /// <summary>
    /// Player ability set runtime summary.
    /// </summary>
    [Serializable]
    public class PlayerAbilitySet : AbilitySet {
        [Tooltip("Primary cooldown ability data summary.")]
        [SerializeField] public CooldownAbilitySummary primary;
        [Tooltip("Secondary cooldown ability data summary.")]
        [SerializeField] public CooldownAbilitySummary secondary;
        [Tooltip("Tertiary cooldown ability data summary.")]
        [SerializeField] public CooldownAbilitySummary tertiary;

        /// <summary>
        /// Setup player ability summary data.
        /// </summary>
        public PlayerAbilitySet(PlayerAbilitySetSO abilitySet, EntityBody eb) : base(abilitySet.abilitySetName, abilitySet.movement, abilitySet.healthSettings, eb) {
            if (abilitySet.primary != null)
                primary = new(abilitySet.primary, eb);

            if (abilitySet.secondary != null)
                secondary = new(abilitySet.secondary, eb);

            if (abilitySet.tertiary != null)
                tertiary = new(abilitySet.tertiary, eb);
        }
    }
}