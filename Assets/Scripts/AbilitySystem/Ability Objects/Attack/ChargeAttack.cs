using System.Collections;
using System.Collections.Generic;

using UnityEditor.EditorTools;

using UnityEngine;

namespace AbilitySystem {
    /// <summary>
    /// Movement-based ability that overrides player input while allowing original input to influence the charge direction for a short duration.
    /// </summary>
    [CreateAssetMenu(fileName = "ChargeAttack", menuName = MenuAssetNames.CooldownAbility + "/Charge Attack")]
    public class ChargeAttack : CooldownAbilitySO {
        [Tooltip("Area collider check for where the charge hits. Can't hit the same target multiple times.")]
        [SerializeField] AreaColliderCheck colliderCheck;
        [Tooltip("The duration of the charge.")]
        [SerializeField, Range(0, 2)] float maxChargeDuration = 1;
        [Tooltip("The damage of the charge.")]
        [SerializeField, Range(0, 2)] int damage = 20;

        /// <summary>
        /// Return charge data template to be used by the entity.
        /// </summary>
        public override AbilityData AbilityDataSetup(EntityBody entityBody) => new ChargeAttackData(charges, cooldown);

        /// <summary>
        /// Core ability activated after charge button press.
        /// </summary>
        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {

            /// Setup data
            ChargeAttackData cad = (ChargeAttackData)data;
            AbilityControllerValues controlValues = entityBody.iAbility.GetInputValues;
            float time = 0;
            int count; IHealth ihealth;
            cad.iHealth.Clear();

            /// Set override movement options
            controlValues.SetMovementTypeToggle(MovementType.Charge, true);
            controlValues.SetDirection(entityBody.bodyHolder.transform.forward, true);
            controlValues.isOverrideActive = true;

            /// Charge while timer is less than 'maxChargeDuration'
            while (time < maxChargeDuration) {
                time += Time.deltaTime;
                count = colliderCheck.GetColliders(entityBody.bodyHolder).Invoke(cad.raycastHits);
                for (int i = 0; i < count; i++)
                    if (cad.raycastHits[i].TryGetComponent(out ihealth) && !cad.iHealth.Contains(ihealth))
                        ihealth.Damage(new EntityDamage(20, entityBody, entityBody.iAbility.GetEntityTeam, EntityDamageType.Heavy));

                yield return null;
            }

            /// Return controls back to player
            controlValues.isOverrideActive = false;
        }
        public override void AnimationEvent(AbilityAnimationEventData animationData, EntityBody entityBody, AbilityData abilityData) { }

        /// <summary>
        /// Base charge attack data to be stored.
        /// </summary>
        public class ChargeAttackData : CooldownData {
            public List<IHealth> iHealth = new List<IHealth>();
            public ChargeAttackData(int charges, float cooldown) : base(charges, cooldown) { }

        }
#if UNITY_EDITOR
        /// <summary>
        /// Draw CheckAreaCollider info for attack range
        /// </summary>
        public override void GizmoEvent(EntityBody entityBody) {
            base.GizmoEvent(entityBody);
            colliderCheck.Gizmo(entityBody.bodyHolder);
        }
#endif
    }
}