using UnityEngine;

using System;
using System.Collections;

namespace AbilitySystem {
    public abstract class CooldownAbilitySO : AbilitySO {
        [Header("Ability Settings")]
        [SerializeField, Range(0.1f, 20)] protected float cooldown;
        [SerializeField, Range(1, 5)] protected int charges;
        protected bool animationPlaying = false;
        [SerializeField] protected AreaColliderCheck attackArea;
        [SerializeField] bool mustRePressToUse = false;
        [SerializeField] protected bool debug = false;
        public override AbilityData AbilityDataSetup(EntityBody entityBody) {
            return new CooldownData(charges, cooldown);
        }
        #region Call Logic
        public override bool Execute(EntityBody entityBody, AbilityData data) {
            CooldownData cdd = (CooldownData)data;
            if (data.usingAbility) {
                if (data.isHoldingInput)
                    OnHold(entityBody, cdd);
                else
                    OnPressWhileUsing(entityBody, cdd);
                return false;
            } else if (entityBody.UsingAbility || cdd.currentCharges <= 0 || (mustRePressToUse && data.isHoldingInput)) {
                return false;
            }

            entityBody.iAbility.GetAbilityController.StartCoroutine(UseAbility(entityBody, cdd));
            return true;
        }
        public override void Startup(EntityBody entityBody, AbilityData data) { }
        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            data.isHoldingInput = false;
            return true;
        }
        protected virtual void OnHold(EntityBody entityBody, CooldownData data) { }
        protected virtual void OnPressWhileUsing(EntityBody entityBody, CooldownData data) { }
        public override void FrameEvent(EntityBody entityBody, AbilityData data) {
            CooldownData cdd = (CooldownData)data;

            if (cdd.currentCharges >= charges) {
                cdd.currentCharges = charges;
                cdd.cooldownDelta = 0;
            } else if (cdd.cooldownDelta > cooldown) {
                cdd.cooldownDelta = 0;
                cdd.currentCharges++;
            } else {
                cdd.cooldownDelta += Time.deltaTime;
            }
        }
        #endregion

        #region Ability Logic
        protected IEnumerator UseAbility(EntityBody entityBody, CooldownData data) {
            data.isHoldingInput = true;
            data.usingAbility = true;
            entityBody.UsingAbility = true;
            data.currentCharges--;
            yield return Ability(entityBody, data);
            data.usingAbility = false;
            entityBody.UsingAbility = false;
            entityBody.MoveOverride = false;
        }
        protected abstract IEnumerator Ability(EntityBody entityBody, CooldownData data);
        protected virtual IEnumerator AttackAnimation(EntityBody entityBody, AbilityData data, AnimationType attackAnimation) {
            animationPlaying = true;
            if (entityBody.animatorManager == null) {
                AnimationEvent(new AbilityAnimationEventData(), entityBody, data);
                yield break;
            }
            yield return entityBody.animatorManager.InitiateOneOffAnimation(
                null,
                null,
                (AbilityAnimationEventData animationData) => AnimationEvent(animationData, entityBody, data),
                () => animationPlaying = false,
                attackAnimation,
                false
            );
            while (animationPlaying)
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

        public override void FrameEvent(EntityBody entityBody) =>
            abilitySO?.FrameEvent(entityBody, AbilityData);
        public override void OnDrawGizmos(EntityBody entityBody) =>
            abilitySO?.GizmoEvent(entityBody);
        public CooldownAbilitySummary(CooldownAbilitySO m, EntityBody eb) {
            abilitySO = m;
            AbilityData = m.AbilityDataSetup(eb);
        }
    }
    public class CooldownData : AbilityData {
        public float cooldownDelta;
        public int currentCharges;
        public bool isRecharging = false;
        public Collider[] raycastHits;
        public CooldownData(int charges, float cooldown) {
            currentCharges = charges;
            cooldownDelta = cooldown;
            raycastHits = new Collider[10];
        }
    }
}