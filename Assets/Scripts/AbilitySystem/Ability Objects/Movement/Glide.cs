using System;

using UnityEngine;

namespace AbilitySystem {

    /// <summary>
    /// Glide ability for movement script.
    /// </summary>
    [CreateAssetMenu(fileName = "GlideMovement", menuName = MenuAssetNames.MovementAbility + "/Glide Movement", order = -1)]
    public class Glide : MovementSO {

        [Header("Horizontal Velocity")]
        [Tooltip("Speed multiplier.")]
        [SerializeField, Min(0)] float speedMult;
        [Tooltip("Total speed that can be reached.")]
        [SerializeField, Min(0)] float maxSpeed;
        [Tooltip("Speed that player can turn around.")]
        [SerializeField, Min(0)] float changeDirectionSpeedMultiplier;
        [Tooltip("Acceleration of the player.")]
        [SerializeField, Min(0)] float acceleration;
        [Tooltip("Deceleration of the player.")]
        [SerializeField, Min(0)] float deceleration;

        [Header("Vertical Velocity")]
        [Tooltip("Max time the player can glide.")]
        [SerializeField, Min(0)] float maxGlideTime;
        [Tooltip("The fall speed of the player at the start of the glide.")]
        [SerializeField, Min(0)] float glideFallSpeed;
        [Tooltip("The fall speed of the player at the end of the glide curve.")]
        [SerializeField, Min(1)] float maxFallSpeed;
        [Tooltip("How quickly the entity will correct from fall speed to glide speed.")]
        [SerializeField] float fallToGlideAcceleration;
        [Tooltip("The fall speed of gliding over time, 0->1 on and x and y being the difference between fallspeed and glidefallspeed, and that value changing over time between start and end.")]
        [SerializeField] protected AnimationCurve glideFallSpeedOverTime;
        [Space]
        [Header("Wall Climbing")]
        [Tooltip("Wall layers to check.")]
        [SerializeField] protected LayerMask wallCheckLayers;
        [Tooltip("Wall climb event.")]
        [SerializeField] protected string onClimb;


        /// <summary>
        /// Setup data.
        /// </summary>
        public override AbilityData AbilityDataSetup(EntityBody entityBody) => new TransformingPlayerData();

        /// <summary>
        /// Base movement that controls gliding.
        /// </summary>
        public override bool NormalMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            TransformingPlayerData moveData = (TransformingPlayerData)data;
            if (inpVals.Direction.y < 0.5f || entityBody.isGrounded) {
                if (entityBody.isGrounded) moveData.glideTime = 0;
                entityBody.iAbility.InputTransitionName("Kuhaku");
                return true;
            }
            /// Prepare for spider model on wall climb
            if (moveData.isJumpButtonRePressed && GenericPlayerMovement.CheckForWall(entityBody, entityBody.bodyHolder.transform.position, entityBody.bodyHolder.transform.forward.normalized * 0.05f, ref moveData.wallClimbObj, ref moveData.wallRaycastHits, wallCheckLayers, onClimb, moveData)) {
                moveData.velocity = Vector3.zero;
                moveData.fallSpeed = 0;
                moveData.isJumpButtonRePressed = false;
                moveData.isJumpingButtonPressed = true;
                moveData.releasedOnJump = false;
                entityBody.iAbility.OnMoveEntity(moveData.wallClimbObj.point - entityBody.bodyHolder.transform.position);
                Debug.Log(moveData.velocity);
                return false;
            }
            moveData.velocity = AccelerationMovement(inpVals.Direction, moveData.velocity);
            GlideEvent(entityBody, moveData);
            entityBody.iAbility.OnMoveEntity(moveData.velocity * speedMult * Time.deltaTime);

            return true;
        }

        /// <summary>
        /// Horizontal movement on player input.
        /// </summary>
        private Vector3 AccelerationMovement(Vector3 inputDir, Vector3 currentVelocity) {
            Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);

            /// If move input
            if (inputDir.sqrMagnitude > 0.001f) {
                inputDir.Normalize();

                /// Acceleration calculations based on current travel direction
                Vector3 currentDir = horizontalVelocity.sqrMagnitude > 0.001f
                    ? horizontalVelocity.normalized
                    : inputDir;
                float directionMultiplier = Mathf.Lerp(
                    changeDirectionSpeedMultiplier,
                    1f,
                    Vector3.Dot(currentDir, inputDir)
                );
                float accel = acceleration * directionMultiplier * Time.deltaTime;

                /// Set horizontal velocity
                horizontalVelocity += inputDir * accel;
                if (horizontalVelocity.magnitude > maxSpeed)
                    horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.ClampMagnitude(horizontalVelocity, maxSpeed), deceleration);
            }

            /// If no Input
            else {
                horizontalVelocity = Vector3.MoveTowards(
                    horizontalVelocity,
                    Vector3.zero,
                    deceleration * Time.deltaTime
                );
            }

            return new Vector3(horizontalVelocity.x, 0, horizontalVelocity.z);
        }

        /// <summary>
        /// Set glide speed on glide
        /// </summary>        
        private void GlideEvent(EntityBody entityBody, TransformingPlayerData pmd) {
            pmd.glideTime += Time.deltaTime;
            pmd.fallSpeed = Mathf.MoveTowards(
                pmd.fallSpeed, // from
                Mathf.Lerp(-maxFallSpeed, -glideFallSpeed, Mathf.Clamp01(glideFallSpeedOverTime.Evaluate(pmd.glideTime / maxGlideTime))), // to
                fallToGlideAcceleration * Time.deltaTime //rate
            );
            pmd.velocity.y = pmd.fallSpeed;
        }

        #region Unused functions
        public override bool AutoTrackMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) => throw new NotImplementedException();
        public override bool ChargeMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) => throw new NotImplementedException();
        public override bool FlightMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) => throw new NotImplementedException();
        public override bool PassEvent(EntityBody entityBody, AbilityData data) => throw new NotImplementedException();
        #endregion
    }
}