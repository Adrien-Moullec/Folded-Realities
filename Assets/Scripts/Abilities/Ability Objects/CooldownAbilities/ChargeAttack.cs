using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "ChargeAttack", menuName = MenuAssetNames.CooldownAbility + "/Charge Attack")]
    public class ChargeAttack : CooldownAbilitySO {
        [SerializeField, Min(0)] int chargeDamage = 10;
        [SerializeField, Range(0, 10)] float chargeUpDuration = 1;
        [SerializeField, Range(0, 2)] float maxChargeDuration = 1;
        [SerializeField, Range(2, 10)] float chargeDownDuration = 1;

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
            AbilityControllerValues controlValues = entityBody.iAbility.GetInputValues;
            float time = 0;
            controlValues.isOverrideActive = true;
            controlValues.SetMovementTypeToggle(MovementType.Charge, true);
            controlValues.SetDirection(entityBody.modelPrefab.transform.forward, true);
            while (time < maxChargeDuration) {
                time += Time.deltaTime;
                yield return null;
            }
            controlValues.isOverrideActive = false;
        }

        protected override void OnHold(EntityBody entityBody, CooldownData data) {

        }

        protected override void OnPressWhileUsing(EntityBody entityBody, CooldownData data) {

        }

        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            return true;
        }
        public class ChargeAttackData : CooldownData {
            //public TimelineEvent[] chargeTimeline;
            public ChargeAttackData(int charges, float cooldown) : base(charges, cooldown) {

            }

        }
    }
}