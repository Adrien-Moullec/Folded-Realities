using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "ChargeAttack", menuName = MenuAssetNames.CooldownAbility + "/Charge Attack")]
    public class ChargeAttack : CooldownAbilitySO {
        [Header("Animations")]
        [Tooltip("The animation that will play for this ability.")]
        [SerializeField] AbilityAnimation chargeUpChargeAnim;
        [SerializeField] AbilityAnimation chargeAnim;
        [SerializeField] AbilityAnimation chargeDownChargeAnim;
        [SerializeField, Min(0)] int chargeDamage = 10;
        [SerializeField, Range(0, 10)] float chargeUpDuration = 1;
        [SerializeField, Range(0, 2)] float maxChargeDuration = 1;
        [SerializeField, Range(2, 10)] float chargeDownDuration = 1;

        public override (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup() =>
            new (AbilityAnimation, WrapMode)[]
            {
                (chargeAnim, WrapMode.ClampForever),
                (chargeUpChargeAnim, WrapMode.Loop)
            };

        public override AbilityData AbilityDataSetup(EntityBody entityBody) {
            ChargeAttackData cad = new ChargeAttackData(charges, cooldown);
            // cad.chargeTimeline = new TimelineEvent[] {
            //new TimelineEvent(entityBody.animationComponent, chargeUpChargeAnim, 0, chargeUpChargeAnim.length),
            //new TimelineEvent(entityBody.animationComponent, chargeUpChargeAnim, chargeUpChargeAnim.length, chargeUpChargeAnim.length + maxChargeDuration, false, () => OnCharge(entityBody, cad), () => StopCharge(entityBody, cad)),
            //new TimelineEvent(entityBody.animationComponent, chargeUpChargeAnim, 0, 1),
            //new TimelineEvent(entityBody.animationComponent, chargeUpChargeAnim, 0, 1, false, () => OnCharge(entityBody, cad), () => StopCharge(entityBody, cad)),
            //};
            return cad;
        }

        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
            ChargeAttackData cad = (ChargeAttackData)data;
            float time = 0;
            entityBody.iAbility.GetInputValues.isAccelerating = false;
            if (entityBody.iAbility.GetInputValues.inputDirection == Vector3.zero)
                entityBody.iAbility.GetInputValues.inputDirection = entityBody.modelPrefab.transform.forward;
            entityBody.iAbility.GetInputValues.LockValues = true;
            while (time < maxChargeDuration) {
                time += Time.deltaTime;
                yield return null;
            }
            entityBody.iAbility.GetInputValues.LockValues = false;
            entityBody.iAbility.GetInputValues.isAccelerating = true;
        }

        public class ChargeAttackData : CooldownData {
            //public TimelineEvent[] chargeTimeline;
            public ChargeAttackData(int charges, float cooldown) : base(charges, cooldown) {

            }

        }
    }
}