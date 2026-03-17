using System;

using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Ability Set", menuName = MenuAssetNames.AbilitySet + "/Player Ability Set", order = 0)]
    public class PlayerAbilitySetSO : AbilitySetSO {
        [SerializeField] public CooldownAbilitySO light;
        [SerializeField] public CooldownAbilitySO heavy;
        [SerializeField] public CooldownAbilitySO primary;

        [Space]
        [Header("Transition options")]
        [SerializeField] public AbilityAnimation transitionAnimation;

        public override void SetupAnimations(Animation anim) {
            base.SetupAnimations(anim);
            transitionAnimation.Setup(anim, WrapMode.ClampForever);
            if (light != null) AssignAnimations(anim, light);
            if (heavy != null) AssignAnimations(anim, heavy);
            if (primary != null) AssignAnimations(anim, primary);
        }
    }

    [Serializable]
    public class PlayerAbilitySet : AbilitySet {
        [SerializeField] public AbilityAnimation transitionAnimation; [SerializeField] public ActivatedAbilitySummary light;
        [SerializeField] public ActivatedAbilitySummary heavy;
        [SerializeField] public ActivatedAbilitySummary primary;

        public PlayerAbilitySet(PlayerAbilitySetSO abilitySet) : base(abilitySet.name, abilitySet.movement, abilitySet.healthSettings) {
            if (abilitySet.light != null)
                light = new(abilitySet.light);

            if (abilitySet.heavy != null)
                heavy = new(abilitySet.heavy);

            if (abilitySet.primary != null)
                primary = new(abilitySet.primary);
        }
    }
}