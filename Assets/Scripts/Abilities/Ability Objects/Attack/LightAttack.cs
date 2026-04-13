using System;
using System.Collections;

using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Light Attack", menuName = MenuAssetNames.AttackAbility + "/Light attack")]
    public class LightAttack : CooldownAbilitySO {
        [Tooltip("The effected animated transforms and the animation timeline point of the game mechanic event.")]
        [SerializeField, Range(0, 1)] float deltaEvent;

        [SerializeField] int damage = 10;
        public override AbilityData AbilityDataSetup(EntityBody entityBody) {
            return new CooldownData(charges, cooldown);
        }

        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
            yield return AttackAnimation(entityBody, data, AnimationType.Attack1);
        }

        public override void AnimationEvent(AbilityAnimationEventData animationData, EntityBody entityBody, AbilityData abilityData) {
            Damage(entityBody);
        }


        protected override void OnHold(EntityBody entityBody, CooldownData data) {

        }

        protected override void OnPressWhileUsing(EntityBody entityBody, CooldownData data) {

        }

        private void Damage(EntityBody entityBody) {
            Collider[] colliders = Physics.OverlapSphere(entityBody.attackCubeArea.transform.position, entityBody.attackCubeArea.size.x);
            foreach (var n in colliders)
                if (n.transform.TryGetComponent(out IHealth iHealth))
                    if (iHealth != entityBody.iHealth)
                        iHealth.Damage(
                            new EntityDamage(
                                damage,
                                entityBody,
                                entityBody.iAbility.GetEntityTeam,
                                EntityDamageType.Melee
                            )
                        );
        }
    }
}