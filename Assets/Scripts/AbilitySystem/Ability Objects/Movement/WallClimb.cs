using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "WallClimbMovement", menuName = MenuAssetNames.MovementAbility + "/Wall Climb")]
    public class WallClimb : MovementSO {

        [Header("Wall Climb")]
        [SerializeField] LayerMask wallCheckLayers;
        [SerializeField] float turnSpeed = 1;
        [SerializeField] float speedMultiplier = 5;
        [SerializeField] float wallJumpHeightMultiplier = 2;

        public override AbilityData AbilityDataSetup(EntityBody entityBody) => new TransformingPlayerData();

        #region Alternate movement methods
        public override bool AutoTrackMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }

        public override bool ChargeMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }

        public override bool FlightMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }
        #endregion

        public override bool NormalMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            TransformingPlayerData moveData = (TransformingPlayerData)data;

            #region Wall and jump checks
            moveData.isJumpingButtonPressed = inpVals.Direction.y > 0.5f;
            if (!moveData.isJumpingButtonPressed) moveData.releasedOnJump = true;
            else moveData.velocity = Vector3.zero;
            moveData.chargeDirection = inpVals.Direction;

            bool hasWall = GenericPlayerMovement.CheckForWall(entityBody, entityBody.bodyHolder.transform.position, -moveData.wallClimbObj.normal * 4, ref moveData.wallClimbObj, ref moveData.wallRaycastHits, wallCheckLayers, "");

            // Release climb
            if (!hasWall || (moveData.isJumpingButtonPressed && moveData.releasedOnJump)) {
                moveData.velocity = moveData.wallClimbObj.normal + Vector3.up * wallJumpHeightMultiplier;
                moveData.releasedOnJump = false;
                moveData.isClimbing = false;

                entityBody.iAbility.OnRotateEntity(moveData.wallClimbObj.normal);
                entityBody.iAbility.OnMoveEntity(moveData.velocity);

                entityBody.iAbility.OnAbilityEvent("Kuhaku");
                return true;
            }
            #endregion

            #region Direction Math
            Vector3 wallNormal = moveData.wallClimbObj.normal.normalized;
            Vector3 wallRight = Vector3.Cross(Vector3.up, wallNormal).normalized;
            Vector3 wallUp = Vector3.Cross(wallNormal, wallRight).normalized; //Vector3.up
            Vector3 input = inpVals.Direction;
            input.y = 0; input.Normalize();

            float horizontal = Vector3.Dot(input, wallRight);
            float vertical = Vector3.Dot(input, -wallNormal);

            Vector3 dir = wallRight * horizontal + wallUp * vertical;
            moveData.velocity = Vector3.MoveTowards(moveData.velocity, dir - wallNormal, turnSpeed);

            //Debug.Log(moveData.velocity);
            moveData.velocity.Normalize();
            entityBody.iAbility.OnMoveEntity(moveData.velocity * speedMultiplier * Time.deltaTime, false);
            entityBody.iAbility.OnRotateEntity(-wallNormal);
            #endregion

            #region Model Settings
            if (dir != Vector3.zero) {
                entityBody.prefab.transform.rotation = Quaternion.RotateTowards(entityBody.prefab.transform.rotation, Quaternion.LookRotation(-moveData.wallClimbObj.normal, moveData.velocity), 0.8f);
            }
            if (entityBody.animatorManager.gameObject.activeSelf) {
                entityBody.animatorManager?.SetMovement(moveData.velocity.magnitude / (dir - wallNormal).magnitude, 0, true);
                entityBody.animatorManager?.SetMovementState();
            }
            #endregion

            return false;
        }

        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            throw new System.NotImplementedException();
        }
    }
}