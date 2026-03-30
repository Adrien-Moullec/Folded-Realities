using UnityEngine;

using System;
using System.Collections;

namespace AbilitySystem {
    public abstract class CooldownAbilitySO : AbilitySO {
        [Header("Ability Settings")]
        [SerializeField, Range(0.1f, 20)] protected float cooldown;
        [SerializeField, Range(1, 5)] protected int charges;
        public override AbilityData AbilityDataSetup() => new CooldownData(charges, cooldown);
        public static IEnumerator OnUseCooldownSequence(CooldownData data, float cooldown, int maxCharges) {
            data.currentCharges--;
            data.isRecharging = true;
            data.cooldownDelta = cooldown;
            while (data.currentCharges < maxCharges) {
                yield return null;
                data.cooldownDelta -= Time.deltaTime;

                if (data.cooldownDelta <= 0) {
                    data.currentCharges++;
                    data.cooldownDelta = cooldown;
                }
            }
            data.isRecharging = false;
        }
        public virtual bool TryUseAbility(EntityBody entityBody, CooldownData data) {
            if (data.currentCharges <= 0) return false;
            entityBody.iAbility.ActivateIenumerator(OnUseCooldownSequence(data, cooldown, charges));
            entityBody.iAbility.ActivateIenumerator(UseAbility(entityBody, data));
            return true;
        }
        protected IEnumerator UseAbility(EntityBody entityBody, CooldownData data) {
            data.isUsing = true;
            yield return Ability(entityBody, data);
            data.isUsing = false;
        }
        protected abstract IEnumerator Ability(EntityBody entityBody, CooldownData data);
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