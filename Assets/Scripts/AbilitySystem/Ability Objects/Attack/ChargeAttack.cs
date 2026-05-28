using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "ChargeAttack", menuName = MenuAssetNames.CooldownAbility + "/Charge Attack")]
    public class ChargeAttack : CooldownAbilitySO {
        //[SerializeField, Min(0)] int chargeDamage = 10;
        [SerializeField] AreaColliderCheck colliderCheck;
        [SerializeField, Range(0, 2)] float maxChargeDuration = 1;
        [SerializeField, Range(0, 2)] int damage = 20;

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
            int count; IHealth ihealth;
            cad.iHealth.Clear();
            while (time < maxChargeDuration) {
                time += Time.deltaTime;
                count = colliderCheck.GetColliders(entityBody.bodyHolder).Invoke(cad.raycastHits);
                for (int i = 0; i < count; i++) {
                    if (cad.raycastHits[i].TryGetComponent(out ihealth) && !cad.iHealth.Contains(ihealth)) {
                        ihealth.Damage(new EntityDamage(20, entityBody, entityBody.iAbility.GetEntityTeam, EntityDamageType.Heavy));
                    }
                }
                yield return null;
            }
            controlValues.isOverrideActive = false;
        }

        protected override void OnHold(EntityBody entityBody, CooldownData data) {

        }

        protected override void OnPressWhileUsing(EntityBody entityBody, CooldownData data) {

        }

        public override void AnimationEvent(AbilityAnimationEventData animationData, EntityBody entityBody, AbilityData abilityData) {
        }

        public class ChargeAttackData : CooldownData {
            public List<IHealth> iHealth = new List<IHealth>();
            public ChargeAttackData(int charges, float cooldown) : base(charges, cooldown) {

            }

        }
#if UNITY_EDITOR
        public override void GizmoEvent(EntityBody entityBody) {
            base.GizmoEvent(entityBody);
            colliderCheck.Gizmo(entityBody.bodyHolder);
        }
#endif
    }
}