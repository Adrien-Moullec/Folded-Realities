using UnityEngine;

using System;
using System.Collections;

namespace AbilitySystem {
    public abstract class CooldownAbilitySO : AbilitySO {
        [Header("Ability Settings")]
        [SerializeField, Range(0.1f, 20)] protected float cooldown;
        [SerializeField, Range(1, 5)] protected int charges;
        public override AbilityData AbilityDataSetup(EntityBody eb) => new CooldownData(charges, cooldown);
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
        public override bool Execute(EntityBody entityBody, AbilityData data) {
            CooldownData cdd = (CooldownData)data;
            Debug.Log("Execute charge ability");
            if (cdd.currentCharges <= 0) return false;
            entityBody.iAbility.ActivateIenumerator(OnUseCooldownSequence(cdd, cooldown, charges));
            entityBody.iAbility.ActivateIenumerator(UseAbility(entityBody, cdd));
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

        public override void Activate(EntityBody entityBody) {
            abilitySO.Execute(entityBody, AbilityData);
        }

        public ActivatedAbilitySummary(CooldownAbilitySO m, EntityBody eb) {
            abilitySO = m;
            AbilityData = m.AbilityDataSetup(eb);
        }
    }
    public class CooldownData : AbilityData {
        [HideInInspector] internal float cooldownDelta;
        [HideInInspector] internal int currentCharges;
        [HideInInspector] internal bool isRecharging = false;
        [HideInInspector] internal bool isUsing = false;
        public CooldownData(int charges, float cooldown) {
            currentCharges = charges;
            cooldownDelta = cooldown;
        }
    }
}