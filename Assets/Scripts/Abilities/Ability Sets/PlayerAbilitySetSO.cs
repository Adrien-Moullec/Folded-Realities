using System;

using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Ability Set", menuName = MenuAssetNames.AbilitySet + "/Player Ability Set", order = 0)]
    public class PlayerAbilitySetSO : AbilitySetSO {
        [SerializeField] public CooldownAbilitySO light;
        [SerializeField] public CooldownAbilitySO heavy;
        [SerializeField] public CooldownAbilitySO primary;
    }

    [Serializable]
    public class PlayerAbilitySet : AbilitySet {
        [SerializeField] public CooldownAbilitySummary light;
        [SerializeField] public CooldownAbilitySummary heavy;
        [SerializeField] public CooldownAbilitySummary primary;

        public PlayerAbilitySet(PlayerAbilitySetSO abilitySet, EntityBody eb) : base(abilitySet.name, abilitySet.movement, abilitySet.healthSettings, eb) {
            if (abilitySet.light != null)
                light = new(abilitySet.light, eb);

            if (abilitySet.heavy != null)
                heavy = new(abilitySet.heavy, eb);

            if (abilitySet.primary != null)
                primary = new(abilitySet.primary, eb);
        }
    }
}