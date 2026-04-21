using System;

using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Ability Set", menuName = MenuAssetNames.AbilitySet + "/Player Ability Set", order = 0)]
    public class PlayerAbilitySetSO : AbilitySetSO {
        [SerializeField] public CooldownAbilitySO primary;
        [SerializeField] public CooldownAbilitySO secondary;
        [SerializeField] public CooldownAbilitySO tertiary;
    }

    [Serializable]
    public class PlayerAbilitySet : AbilitySet {
        [SerializeField] public CooldownAbilitySummary primary;
        [SerializeField] public CooldownAbilitySummary secondary;
        [SerializeField] public CooldownAbilitySummary tertiary;

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