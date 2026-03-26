using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    public class ChargeAttack : CooldownAbilitySO {
        [Header("Animations")]
        [Tooltip("The animation that will play for this ability.")]
        [SerializeField] AbilityAnimation chargeUpChargeAnim;
        [SerializeField] AbilityAnimation chargeAnim;
        [SerializeField, Min(0)] int chargeDamage = 10;
        [SerializeField, Range(0, 10)] float chargeUpDuration = 1;
        [SerializeField, Range(2, 10)] float chargeDuration = 10;

        public override (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup() =>
            new (AbilityAnimation, WrapMode)[]
            {
                (chargeAnim, WrapMode.ClampForever),
                (chargeUpChargeAnim, WrapMode.Loop)
            };

        public override AbilityData AbilityDataSetup() {
            return base.AbilityDataSetup();
        }


        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
            yield return entityBody.iAbility.RunTimelineWithEvents(
                new TimelineEvent[] {
                    new TimelineEvent(entityBody.animationComponent, chargeUpChargeAnim, 0, chargeUpChargeAnim.length)
                }
            );
        }

        public class ChargeAttackData : CooldownData {

        }
    }
}