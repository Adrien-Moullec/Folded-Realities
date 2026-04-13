using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "ChargeAttack", menuName = MenuAssetNames.CooldownAbility + "/Charge Attack")]
    public class ChargeAttack : CooldownAbilitySO {
        //[SerializeField, Min(0)] int chargeDamage = 10;
        [SerializeField, Range(0, 2)] float maxChargeDuration = 1;

        public override AbilityData AbilityDataSetup(EntityBody entityBody) {
            ChargeAttackData cad = new ChargeAttackData(charges, cooldown);
            return cad;
        }


        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
            ChargeAttackData cad = (ChargeAttackData)data;
            AbilityControllerValues controlValues = entityBody.iAbility.GetInputValues;
            float time = 0;
            controlValues.isOverrideActive = true;
            controlValues.SetMovementTypeToggle(MovementType.Charge, true);
            controlValues.SetDirection(entityBody.bodyHolder.transform.forward, true);
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

        public override void AnimationEvent(AbilityAnimationEventData animationData, EntityBody entityBody, AbilityData abilityData) {
        }

        public class ChargeAttackData : CooldownData {
            public ChargeAttackData(int charges, float cooldown) : base(charges, cooldown) {

            }

        }
    }
}