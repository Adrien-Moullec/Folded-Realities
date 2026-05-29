using System;
using System.Collections;

using UnityEngine;


namespace AbilitySystem {

    /// <summary>
    /// Basic light attack that can be incorporated by the majority of any melee animation.
    /// </summary>
    [CreateAssetMenu(fileName = "Light Attack", menuName = MenuAssetNames.AttackAbility + "/Light attack")]
    public class LightAttack : CooldownAbilitySO {

        [Tooltip("Light attack damage amount.")]
        [SerializeField] int damage = 10;

        #region Ability Events
        /// <summary>
        /// Begin animation on start ability.
        /// </summary>
        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
            yield return AttackAnimation(entityBody, data, AnimationType.Attack1);
        }

        /// <summary>
        /// The event that plays during animation event called.
        /// </summary>
        public override void AnimationEvent(AbilityAnimationEventData animationData, EntityBody entityBody, AbilityData abilityData) => DamageAreaCheck(entityBody, (CooldownData)abilityData, damage, EntityDamageType.Melee);
        #endregion

        #region Unused abstract
        protected override void OnHold(EntityBody entityBody, CooldownData data) { }
        protected override void OnPressWhileUsing(EntityBody entityBody, CooldownData data) { }
        #endregion

#if UNITY_EDITOR
        public override void GizmoEvent(EntityBody entityBody) {
            attackArea.Gizmo(entityBody.bodyHolder);
        }
#endif
    }
}