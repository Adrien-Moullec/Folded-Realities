using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "WallClimbMovement", menuName = MenuAssetNames.MovementAbility + "/Wall Climb")]
    public class WallClimb : GenericPlayerMovement {

        public override bool NormalMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            TransformingPlayerData moveData = (TransformingPlayerData)data;

            moveData.isJumpingButtonPressed = inpVals.Direction.y > 0;
            moveData.chargeDirection = inpVals.Direction;

            bool hasWall = CheckForWall(entityBody, moveData);
            Debug.Log("WALLCLIMB");

            // Release climb
            if (!hasWall || !moveData.isJumpingButtonPressed) {
                Debug.Log(hasWall);
                moveData.isClimbing = false;
                entityBody.iAbility.OnAbilityEvent(onClimbRelease);
                return true;
            }

            Vector3 wallNormal = moveData.wallClimbObj.normal.normalized;
            Vector3 wallRight = Vector3.Cross(Vector3.up, wallNormal).normalized;
            Vector3 wallUp = Vector3.up;// Vector3.Cross(wallNormal, wallRight).normalized;
            Vector3 input = inpVals.Direction;
            input.y = 0; input.Normalize();

            float horizontal = Vector3.Dot(input, wallRight);
            float vertical = Vector3.Dot(input, -wallNormal);

            moveData.velocity = wallRight * horizontal + wallUp * vertical - wallNormal;
            moveData.velocity.Normalize();
            entityBody.iAbility.OnMoveEntity(
                moveData.velocity * speedMultiplier * Time.deltaTime,
                false
            );
            entityBody.iAbility.OnRotateEntity(
                -wallNormal
            );

            return false;
        }
    }
}