using System;
using System.Collections;

using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Light Attack", menuName = MenuAssetNames.AttackAbility + "/Light attack")]
    public class LightAttack : CooldownAbilitySO {
        [Tooltip("The effected animated transforms and the animation timeline point of the game mechanic event.")]
        [SerializeField, Range(0, 1)] float deltaEvent;

        [SerializeField] int damage = 10;

        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
            yield return AttackAnimation(entityBody, data, AnimationType.Attack1);
        }

        public override void AnimationEvent(AbilityAnimationEventData animationData, EntityBody entityBody, AbilityData abilityData) {
            Damage(entityBody, (CooldownData)abilityData);
        }


        protected override void OnHold(EntityBody entityBody, CooldownData data) {

        }

        protected override void OnPressWhileUsing(EntityBody entityBody, CooldownData data) {

        }

        private void Damage(EntityBody entityBody, CooldownData data) {
            int things = attackArea.GetColliders(entityBody.bodyHolder).Invoke(data.raycastHits);

            for (int i = 0; i < things; i++)
                if (data.raycastHits[i].transform.TryGetComponent(out IHealth iHealth))
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
        public override void GizmoEvent(EntityBody entityBody) {
            attackArea.Gizmo(entityBody.bodyHolder);
        }
    }
}