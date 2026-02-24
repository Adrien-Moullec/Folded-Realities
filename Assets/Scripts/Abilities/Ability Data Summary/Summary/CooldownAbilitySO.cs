using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem.Interactions;


namespace AbilitySystem {
    public abstract class CooldownAbilitySO : AbilitySO {
        [Header("Ability Settings")]
        [SerializeField, Range(1, 20)] internal float cooldown;
        [SerializeField, Range(1, 5)] internal int charges;
        public virtual bool TryUseAbility(EntityBody entityBody, CooldownData data) {
            if (data.currentCharges <= 0) return false;
            data.currentCharges--;
            entityBody.iAbility.OnActivateCooldownAbility(Ability(entityBody, data), data, cooldown, charges);
            return true;
        }
        public abstract IEnumerator Ability(EntityBody entityBody, CooldownData data);
        public override AbilityData Setup() {
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
            AbilityData = m.Setup();
        }
    }
}