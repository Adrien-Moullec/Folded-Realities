using System;
using System.Collections;

using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Light Attack", menuName = MenuAssetNames.AttackAbility + "/Light attack")]
    public class LightAttack : CooldownAbilitySO {
        [Header("Animation")]
        [Tooltip("The animation that will play for this ability.")]
        [SerializeField] AbilityAnimation attackAnim;
        [Tooltip("The effected animated transforms and the animation timeline point of the game mechanic event.")]
        [SerializeField, Range(0, 1)] float deltaEvent;

        [SerializeField] int damage = 10;

        public override (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup() =>
            new (AbilityAnimation, WrapMode)[]
            {
                (attackAnim, WrapMode.ClampForever)
            };

        public override AbilityData AbilityDataSetup(EntityBody entityBody) {
            return new CooldownData(charges, cooldown);
        }

        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {

            yield return entityBody.iAbility.RunTimelineWithEvents(
                new TimelineEvent[] {
                    new TimelineEvent(entityBody.animationComponent, attackAnim, 0, attackAnim.length)
                },
                new DeltaEvent[] {
                    new DeltaEvent(() => Damage(entityBody), deltaEvent)
                }
            );
        }

        private void Damage(EntityBody entityBody) {
            Collider[] colliders = Physics.OverlapSphere(entityBody.attackCubeArea.transform.position, entityBody.attackCubeArea.size.x);
            foreach (var n in colliders)
                if (n.transform.TryGetComponent(out IHealth iHealth))
                    if (iHealth != entityBody.iHealth)
                        iHealth.Damage(damage);
        }
    }
}