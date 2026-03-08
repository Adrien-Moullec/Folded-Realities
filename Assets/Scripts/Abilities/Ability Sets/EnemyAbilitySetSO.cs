using System;
using UnityEngine;


namespace AbilitySystem
{
    [CreateAssetMenu(fileName = "Ability Set", menuName = "Origami/Ability Sets/Base Enemy Ability Set", order = 0)]
    public class EnemyAbilitySetSO : AbilitySetSO
    {
        [SerializeField] internal CooldownAbilitySO attack;

        internal override void SetupAnimations(Animation anim)
        {
            base.SetupAnimations(anim);
            if (attack != null) AssignAnimations(anim, attack);
        }
    }

    [Serializable]
    public class EnemyAbilitySet : AbilitySet
    {
        [SerializeField] internal ActivatedAbilitySummary attack;

        public EnemyAbilitySet(EnemyAbilitySetSO abilitySet, Animation anim) : base(abilitySet.name, anim, abilitySet.movement)
        {
            if (abilitySet.attack != null)
                attack = new(abilitySet.attack);
        }
    }
}