using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "WallClimbMovement", menuName = MenuAssetNames.MovementAbility + "/Wall Climb")]
    public class WallClimb : MovementSO {

        [Header("Wall Climb")]
        [SerializeField] LayerMask wallCheckLayers;
        [SerializeField] float turnSpeed = 1;
        [SerializeField] float speedMultiplier = 5;

        public override AbilityData AbilityDataSetup(EntityBody entityBody) => new TransformingPlayerData();

        public override bool AutoTrackMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }

        public override bool ChargeMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }

        public override bool FlightMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }

        public override bool NormalMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            TransformingPlayerData moveData = (TransformingPlayerData)data;

            moveData.isJumpingButtonPressed = inpVals.Direction.y > 0;
            moveData.chargeDirection = inpVals.Direction;

            bool hasWall = GenericPlayerMovement.CheckForWall(entityBody, entityBody.bodyHolder.transform.position, -moveData.wallClimbObj.normal, ref moveData.wallClimbObj, ref moveData.wallRaycastHits, wallCheckLayers, "");

            // Release climb
            if (!hasWall || !moveData.isJumpingButtonPressed) {
                Debug.Log(hasWall);
                moveData.isClimbing = false;
                entityBody.iAbility.OnAbilityEvent("Kuhaku");
                return true;
            }

            Vector3 wallNormal = moveData.wallClimbObj.normal.normalized;
            Vector3 wallRight = Vector3.Cross(Vector3.up, wallNormal).normalized;
            Vector3 wallUp = Vector3.Cross(wallNormal, wallRight).normalized; //Vector3.up
            Vector3 input = inpVals.Direction;
            input.y = 0; input.Normalize();

            float horizontal = Vector3.Dot(input, wallRight);
            float vertical = Vector3.Dot(input, -wallNormal);

            Vector3 dir = wallRight * horizontal + wallUp * vertical;
            moveData.velocity = Vector3.MoveTowards(moveData.velocity, dir - wallNormal, turnSpeed);
            moveData.velocity.Normalize();

            entityBody.iAbility.OnMoveEntity(
                moveData.velocity * speedMultiplier * Time.deltaTime,
                false
            );
            if (dir != Vector3.zero) {
                entityBody.prefab.transform.rotation = Quaternion.RotateTowards(entityBody.prefab.transform.rotation, Quaternion.LookRotation(-moveData.wallClimbObj.normal, moveData.velocity), 0.8f);
            }
            entityBody.iAbility.OnRotateEntity(
                -wallNormal
            );

            return false;
        }

        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            throw new System.NotImplementedException();
        }
    }
}