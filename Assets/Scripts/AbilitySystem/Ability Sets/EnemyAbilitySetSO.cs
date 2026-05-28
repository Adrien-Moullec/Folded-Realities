using System;

using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Ability Set", menuName = MenuAssetNames.AbilitySet + "/Base Enemy Ability Set", order = 0)]
    public class EnemyAbilitySetSO : AbilitySetSO {
        [SerializeField] public CooldownAbilitySO attack;
        [SerializeField] public CooldownAbilitySO attack2;
    }

    [Serializable]
    public class EnemyAbilitySet : AbilitySet {
        [SerializeField] public CooldownAbilitySummary attack;
        [SerializeField] public CooldownAbilitySummary attack2;

        public EnemyAbilitySet(EnemyAbilitySetSO abilitySet, EntityBody eb) : base(abilitySet.name, abilitySet.movement, abilitySet.healthSettings, eb) {
            if (abilitySet.attack != null)
                attack = new(abilitySet.attack, eb);
            if (abilitySet.attack2 != null)
                attack2 = new(abilitySet.attack2, eb);
        }
    }
}