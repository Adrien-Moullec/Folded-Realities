using UnityEngine;

using System;
using System.Collections;

namespace AbilitySystem {
    public abstract class CooldownAbilitySO : AbilitySO {
        [Header("Ability Settings")]
        [SerializeField, Range(0.1f, 20)] protected float cooldown;
        [SerializeField, Range(1, 5)] protected int charges;

        #region Call Logic
        public override bool Execute(EntityBody entityBody, AbilityData data) {
            CooldownData cdd = (CooldownData)data;
            if (data.usingAbility) {
                if (data.isHoldingInput)
                    OnHold(entityBody, cdd);
                else
                    OnPressWhileUsing(entityBody, cdd);
                return false;
            } else if (entityBody.UsingAbility || cdd.currentCharges <= 0 || data.isHoldingInput)
                return false;

            data.isHoldingInput = true;
            data.usingAbility = true;
            entityBody.UsingAbility = true;
            cdd.currentCharges--;
            entityBody.iAbility.GetAbilityController.StartCoroutine(UseAbility(entityBody, cdd));
            return true;
        }
        public override void Startup(EntityBody entityBody, AbilityData data) { }
        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            data.isHoldingInput = false;
            return true;
        }
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
        #endregion

        #region Ability Logic
        protected IEnumerator UseAbility(EntityBody entityBody, CooldownData data) {
            yield return Ability(entityBody, data);
            data.usingAbility = false;
            entityBody.UsingAbility = false;
            entityBody.MoveOverride = false;
        }
        protected abstract IEnumerator Ability(EntityBody entityBody, CooldownData data);
        protected virtual IEnumerator AttackAnimation(EntityBody entityBody, AbilityData data, AnimationType attackAnimation) {
            yield return entityBody.animatorManager.InitiateOneOffAnimation(
                null,
                null,
                (AbilityAnimationEventData animationData) => AnimationEvent(animationData, entityBody, data),
                null,
                attackAnimation,
                false
            );
            yield return null;
        }
        public abstract void AnimationEvent(AbilityAnimationEventData animationEvent, EntityBody entityBody, AbilityData animationType);
        #endregion
    }

    [Serializable]
    public class CooldownAbilitySummary : AbilitySummary {
        [SerializeField] public CooldownAbilitySO abilitySO;

        public override void Activate(EntityBody entityBody, bool abilityPressed) {
            if (abilityPressed) abilitySO?.Execute(entityBody, AbilityData);
            else abilitySO?.PassEvent(entityBody, AbilityData);
        }
        public override void StartUp(EntityBody entityBody) =>
            abilitySO?.Startup(entityBody, AbilityData);

        public override void FrameEvent() =>
            abilitySO?.FrameEvent(AbilityData);

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