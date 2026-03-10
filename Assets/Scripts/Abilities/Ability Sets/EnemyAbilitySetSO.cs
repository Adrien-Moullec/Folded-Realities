using System;

using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Ability Set", menuName = "Origami/Ability Sets/Base Enemy Ability Set", order = 0)]
    public class EnemyAbilitySetSO : AbilitySetSO {
        [SerializeField] public CooldownAbilitySO attack;

        public override void SetupAnimations(Animation anim) {
            base.SetupAnimations(anim);
            if (attack != null) AssignAnimations(anim, attack);
        }
    }

    [Serializable]
    public class EnemyAbilitySet : AbilitySet {
        [SerializeField] public ActivatedAbilitySummary attack;

        public EnemyAbilitySet(EnemyAbilitySetSO abilitySet) : base(abilitySet.name, abilitySet.movement) {
            if (abilitySet.attack != null)
                attack = new(abilitySet.attack);
        }
    }
}