using System;

using UnityEngine;

namespace AbilitySystem {
    /// <summary>
    /// Enemy ability set, designed to have fewer abilities.
    /// </summary>
    [CreateAssetMenu(fileName = "Ability Set", menuName = MenuAssetNames.AbilitySet + "/Base Enemy Ability Set", order = 0)]
    public class EnemyAbilitySetSO : AbilitySetSO {
        [Tooltip("Primary enemy attack object.")]
        [SerializeField] public CooldownAbilitySO attack;
        [Tooltip("Secondary enemy attack object.")]
        [SerializeField] public CooldownAbilitySO attack2;
    }

    /// <summary>
    /// Enemy ability summary for runtime behaviours.
    /// </summary>
    [Serializable]
    public class EnemyAbilitySet : AbilitySet {
        [Tooltip("Primary attack data and object.")]
        [SerializeField] public CooldownAbilitySummary attack;
        [Tooltip("Secondary attack data and object.")]
        [SerializeField] public CooldownAbilitySummary attack2;

        /// <summary>
        /// Enemy ability summary setup
        /// </summary>
        public EnemyAbilitySet(EnemyAbilitySetSO abilitySet, EntityBody eb) : base(abilitySet.name, abilitySet.movement, abilitySet.healthSettings, eb) {
            if (abilitySet.attack != null)
                attack = new(abilitySet.attack, eb);
            if (abilitySet.attack2 != null)
                attack2 = new(abilitySet.attack2, eb);
        }
    }
}