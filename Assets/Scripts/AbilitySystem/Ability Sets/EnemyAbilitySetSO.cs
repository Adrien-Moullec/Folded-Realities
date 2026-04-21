using System;

using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Ability Set", menuName = MenuAssetNames.AbilitySet + "/Base Enemy Ability Set", order = 0)]
    public class EnemyAbilitySetSO : AbilitySetSO {
        [SerializeField] public CooldownAbilitySO attack;
    }

    [Serializable]
    public class EnemyAbilitySet : AbilitySet {
        [SerializeField] public CooldownAbilitySummary attack;

        public EnemyAbilitySet(EnemyAbilitySetSO abilitySet, EntityBody eb) : base(abilitySet.name, abilitySet.movement, abilitySet.healthSettings, eb) {
            if (abilitySet.attack != null)
                attack = new(abilitySet.attack, eb);
        }
    }
}