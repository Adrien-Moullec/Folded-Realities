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
            /*
                        if (entityBody.isGrounded) {
                            tpd.velocity = Vector3.zero;
                            if (inpVals.Direction.y > 0.5f)
                                OnJump(entityBody, tpd);
                        }

                        //Set fallspeed
                        tpd.fallSpeed = Mathf.MoveTowards(tpd.fallSpeed, -jumpSpeed, gravity * Time.deltaTime);
                        tpd.fallSpeed = Mathf.Clamp(tpd.fallSpeed, -jumpSpeed, jumpSpeed);
                        tpd.velocity.y = tpd.fallSpeed;*/

            if (!inpVals.IsCrouching)
                entityBody.iAbility.InputTransitionName("Kuhaku");
            return true;
        }

        private void OnJump(EntityBody entityBody, TransformingPlayerData pmd) {
            pmd.fallSpeed = jumpHeight * 100;
            pmd.isGrounded = false;
            pmd.remainingJumps--;

            pmd.velocity = new Vector3(pmd.velocity.x, 0, pmd.velocity.z).normalized * jumpSpeed;
        }

        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            throw new System.NotImplementedException();
        }

    }
}