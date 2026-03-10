using UnityEngine;

using System;
using System.Collections;

namespace AbilitySystem {
    public abstract class CooldownAbilitySO : AbilitySO {
        [Header("Animation")]
        [Tooltip("The animation that will play for this ability.")]
        [SerializeField] AbilityAnimation cooldownAnim;
        [Tooltip("The effected animated transforms and the animation timeline point of the game mechanic event.")]
        [SerializeField] AnimationDeltaEventInfo deltaEvent;
        [Header("Ability Settings")]
        [SerializeField, Range(1, 20)] internal float cooldown;
        [SerializeField, Range(1, 5)] internal int charges;
        public override (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup() => new (AbilityAnimation, WrapMode)[]
            {
                (cooldownAnim, WrapMode.Once)
            };

        public virtual bool TryUseAbility(EntityBody entityBody, CooldownData data) {
            if (data.currentCharges <= 0) return false;
            (IEnumerator, AnimationDeltaEventInfo)[] deltaEvents = {
                    (Ability(entityBody, data), deltaEvent)
                };
            entityBody.iAbility.OnActivateCooldownAbility(
                cooldownAnim,
                entityBody.animationComponent,
                new (IEnumerator, AnimationDeltaEventInfo)[]
                {
                    (Ability(entityBody, data), deltaEvent)
                },
                data, cooldown, charges);
            return true;
        }
        protected abstract IEnumerator Ability(EntityBody entityBody, CooldownData data);
        public override AbilityData AbilityDataSetup() {
            CooldownData ad = new(charges, cooldown);
            ad.currentCharges = charges;
            ad.cooldownDelta = cooldown;
            return ad;
        }
    }

    [Serializable]
    public class ActivatedAbilitySummary : AbilitySummary {
        [SerializeField] internal CooldownAbilitySO abilitySO;

        internal bool Activate(EntityBody entityBody) {
            return abilitySO.TryUseAbility(entityBody, (CooldownData)AbilityData);
        }

        public ActivatedAbilitySummary(CooldownAbilitySO m) {
            abilitySO = m;
            AbilityData = m.AbilityDataSetup();
        }
    }
}