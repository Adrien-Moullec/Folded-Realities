using System;

using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "FrogMovement", menuName = MenuAssetNames.MovementAbility + "/Frog Movement", order = -1)]
    public class FrogJump : MovementSO {

        [Header("Jump Magnitude")]
        [SerializeField] protected float jumpSpeed = 3f;
        [SerializeField] protected float jumpHeight = 3f;
        [SerializeField] protected float gravity = 3f;

        public override AbilityData AbilityDataSetup(EntityBody eb)
            => new TransformingPlayerData();


        public override bool AutoTrackMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new NotImplementedException();
        }

        public override bool ChargeMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new NotImplementedException();
        }

        public override bool FlightMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new NotImplementedException();
        }

        public override bool NormalMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            TransformingPlayerData tpd = (TransformingPlayerData)data;

            if (entityBody.isGrounded && tpd.velocity.y <= 0.2f) {
                tpd.velocity = Vector3.zero;
                if (inpVals.Direction.y > 0.5f)
                    OnJump(entityBody, tpd, inpVals.Direction);
                if (!inpVals.IsCrouching) entityBody.iAbility.InputTransitionName("Kuhaku");
            } else {
                tpd.velocity.y -= gravity * Time.deltaTime;
            }

            entityBody.iAbility.OnMoveEntity(tpd.velocity * Time.deltaTime);
            return true;
        }

        private void OnJump(EntityBody entityBody, TransformingPlayerData pmd, Vector3 dir) {
            pmd.velocity = new Vector3(dir.x, 0, dir.z).normalized * jumpSpeed;
            pmd.velocity.y = jumpHeight;
        }

        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            throw new System.NotImplementedException();
        }

    }
}