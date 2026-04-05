using UnityEngine;

using System;
using System.Collections;

namespace AbilitySystem {
    public abstract class CooldownAbilitySO : AbilitySO {
        [Header("Ability Settings")]
        [SerializeField, Range(0.1f, 20)] protected float cooldown;
        [SerializeField, Range(1, 5)] protected int charges;

        public override bool Execute(EntityBody entityBody, AbilityData data) {
            CooldownData cdd = (CooldownData)data;
            if (data.usingAbility) {
                if (data.isHoldingInput)
                    OnHold(entityBody, cdd);
                else
                    OnPressWhileUsing(entityBody, cdd);
                return false;
            } else if (cdd.currentCharges <= 0)
                return false;

            cdd.currentCharges--;
            entityBody.iAbility.ActivateIenumerator(UseAbility(entityBody, cdd));
            return true;
        }
        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            CooldownData cdd = (CooldownData)data;
            cdd.isHoldingInput = false;
            return true;
        }
        protected IEnumerator UseAbility(EntityBody entityBody, CooldownData data) {
            data.usingAbility = true;
            yield return Ability(entityBody, data);
            data.isHoldingInput = true;
            data.usingAbility = false;
        }
        protected abstract IEnumerator Ability(EntityBody entityBody, CooldownData data);
        protected abstract void OnHold(EntityBody entityBody, CooldownData data);
        protected abstract void OnPressWhileUsing(EntityBody entityBody, CooldownData data);
        public override void FrameEvent(AbilityData abData) {
            CooldownData data = (CooldownData)abData;

            if (data.currentCharges >= charges) {
                data.currentCharges = charges;
                data.cooldownDelta = 0;
            } else if (data.cooldownDelta > cooldown) {
                data.cooldownDelta = 0;
                data.currentCharges++;
            } else {
                data.cooldownDelta += Time.deltaTime;
            }
        }
    }

    [Serializable]
    public class CooldownAbilitySummary : AbilitySummary {
        [SerializeField] public CooldownAbilitySO abilitySO;

        public override void Activate(EntityBody entityBody, bool abilityPressed) {
            if (abilityPressed) abilitySO.Execute(entityBody, AbilityData);
            else abilitySO.PassEvent(entityBody, AbilityData);
            abilitySO.FrameEvent(AbilityData);
        }
        public override void FrameEvent() =>
            abilitySO.FrameEvent(AbilityData);


        public CooldownAbilitySummary(CooldownAbilitySO m, EntityBody eb) {
            abilitySO = m;
            AbilityData = m.AbilityDataSetup(eb);
        }
    }
    public class CooldownData : AbilityData {
        public float cooldownDelta;
        public int currentCharges;
        public bool isRecharging = false;
        public CooldownData(int charges, float cooldown) {
            currentCharges = charges;
            cooldownDelta = cooldown;
        }
    }
}