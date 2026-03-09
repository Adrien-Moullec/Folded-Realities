using System;
using UnityEngine;
using UnityEditor;

namespace AbilitySystem
{
    [CreateAssetMenu(fileName = "Ability Set", menuName = "Origami/Ability Sets/Player Ability Set", order = 0)]
    public class PlayerAbilitySetSO : AbilitySetSO
    {
        [SerializeField] internal CooldownAbilitySO light;
        [SerializeField] internal CooldownAbilitySO heavy;
        [SerializeField] internal CooldownAbilitySO primary;

        internal override void SetupAnimations(Animation anim)
        {
            base.SetupAnimations(anim);
            if (light != null) AssignAnimations(anim, light);
            if (heavy != null) AssignAnimations(anim, heavy);
            if (primary != null) AssignAnimations(anim, primary);
        }
    }

    [Serializable]
    public class PlayerAbilitySet : AbilitySet
    {
        [SerializeField] internal ActivatedAbilitySummary light;
        [SerializeField] internal ActivatedAbilitySummary heavy;
        [SerializeField] internal ActivatedAbilitySummary primary;

        public PlayerAbilitySet(PlayerAbilitySetSO abilitySet, Animation anim) : base(abilitySet.name, abilitySet.movement)
        {
            if (abilitySet.light != null)
                light = new(abilitySet.light);

            if (abilitySet.heavy != null)
                heavy = new(abilitySet.heavy);

            if (abilitySet.primary != null)
                primary = new(abilitySet.primary);
        }
    }
}