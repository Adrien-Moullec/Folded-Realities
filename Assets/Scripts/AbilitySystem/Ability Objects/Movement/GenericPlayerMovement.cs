using UnityEngine;

namespace AbilitySystem {

    /// <summary>
    /// Advanced movement script for most ground characters/ai 
    /// </summary>
    [CreateAssetMenu(fileName = "GeneralMovement", menuName = MenuAssetNames.MovementAbility + "/General Movement", order = -1)]
    public class GenericPlayerMovement : MovementSO {

        [Space]
        [Header("Speed Settings")]

        [Tooltip("Adjust the overall speed.")]
        [SerializeField] protected float speedMultiplier = 1;

        [Tooltip("If the entity is travelling a different direction to the input movement, this value controls how quickly the entity moves back on themselves.")]
        [SerializeField, Min(1)] protected float changeDirectionSpeedMultiplier = 3;

        [Tooltip("Acceleration from stopped to walk/run speed.")]
        [SerializeField, Min(0.01f)] protected float acceleration = 1;

        [Tooltip("Deceleration when stopping.")]
        [SerializeField, Min(0.01f)] protected float deceleration = 8f;

        [Tooltip("Run acceleration multiplier.")]
        [SerializeField] protected float runAccelerationMultiplier = 1;

        [Tooltip("Acceleration while falling.")]
        [SerializeField, Min(0.01f)] protected float accelerationWhileFalling = 0.5f;
        [Tooltip("Deceleration while falling.")]
        [SerializeField, Min(0.01f)] protected float decelerationWhileFalling = 0.5f;
        [Tooltip("Deceleration while grounded. This lets the entity model keep some momentum if they slide off a ledge.")]
        [SerializeField, Min(0.01f)] protected float decelerationWhileGrounded = 5f;
        [Tooltip("Turn speed of the entity.")]
        [SerializeField, Min(0.01f)] protected float turnSpeed = 10f;
        [Tooltip("Run speed multiplier.")]
        [SerializeField] protected float runSpeedMultiplier = 1.5f;
        [Tooltip("Charge speed.")]
        [SerializeField] protected float chargeSpeedMultiplier = 2f;

        [Space]
        [Header("Speed References")]
        protected static float baseSpeed = 5f;
        protected float walkSpeed => baseSpeed * speedMultiplier;
        protected float runSpeed => baseSpeed * speedMultiplier * runSpeedMultiplier;
        protected float chargeSpeed => baseSpeed * speedMultiplier * chargeSpeedMultiplier;

        [Header("Jump Settings")]
        [Tooltip("Controls if entity can jump.")]
        [SerializeField] protected bool canJump = true;
        [Tooltip("Controls if entity can jump.")]
        [SerializeField, Range(0, 5)] protected int doubleJumpCount = 1;
        [Tooltip("Jump velocity.")]
        [SerializeField, Min(0)] protected float jumpSpeed = 7f;
        [Tooltip("Max fall speed.")]
        [SerializeField, Min(0)] protected float maxFallSpeed = 25f;
        [Tooltip("Grounded fall speed.")]
        [SerializeField, Min(0)] protected float groundedFallSpeed = 2f;

        [Space]
        [Header("Glide Settings")]
        [SerializeField] protected bool canGlide = false;
        [Tooltip("The horizontal speed while gliding.")]
        [SerializeField] protected float glideHorizontalSpeed = 1;
        [Tooltip("The glide horizontal acceleration on input.")]
        [SerializeField] protected float glideAcceleration = 1;
        [Tooltip("The glide horizontal deceleration with no input.")]
        [SerializeField] protected float glideDeceleration = 1;
        [Tooltip("Max Glide Time.")]
        [SerializeField, Min(0)] protected float maxGlideTime = 5;
        [Tooltip("The fall speed of gliding.")]
        [SerializeField] protected float glideFallSpeed = 1;
        [Tooltip("The fall speed of gliding over time, 0->1 on and x and y being the difference between fallspeed and glidefallspeed, and that value changing over time between start and end.")]
        [SerializeField] protected AnimationCurve glideFallSpeedOverTime;
        [Tooltip("The speed to decelerate into glide fallspeed.")]
        [SerializeField] protected float fallToGlideAcceleration = 2;

        [Space]
        [Header("Wall Climbing")]
        [SerializeField] protected AreaColliderCheck wallCheckArea;
        [SerializeField] protected LayerMask wallCheckLayers;

        [Space]
        [Header("Physics")]

        [Tooltip("Gravity acceleration.")]
        [SerializeField, Min(0)] protected float gravity = 60f;

        [Space]
        [Header("Charge")]
        protected float chargeChangeDirectionAmount = 1;
        public bool DEBUG = false;

        [Space]
        [Header("Movement Events")] // Events that get pinged to iAbility when certain conditions are met
        [SerializeField] protected bool DebugLog = false;
        [SerializeField] protected string onHitGround;
        [SerializeField] protected string onGlide;
        [SerializeField] protected string onFreeFall;
        [SerializeField] protected string onHitWall;
        [SerializeField] protected string onCrouch;
        [SerializeField] protected string onUncrouch;
        [SerializeField] protected string onClimb;
        [SerializeField] protected string onClimbRelease;
        #region Setup

        /// <summary>
        /// The data that is used in generic movement scriptable ability
        /// </summary>
        /// <param name="eb"></param>
        /// <returns></returns>
        public override AbilityData AbilityDataSetup(EntityBody eb) {
            return new TransformingPlayerData();
        }
        #endregion

        #region Movement Logic
        /// <summary>
        /// Logic called to setup ability 
        /// </summary>
        /// <param name="entityBody"></param>
        /// <param name="data"></param>
        public override void Startup(EntityBody entityBody, AbilityData data) {
            TransformingPlayerData pmd = (TransformingPlayerData)data;
            if (pmd.isGrounded) entityBody.iAbility.OnAbilityEvent(onHitGround);
            if (!pmd.isGrounded && !pmd.isJumpButtonRePressed) entityBody.iAbility.OnAbilityEvent(onFreeFall);
        }
        /// <summary>
        /// Older code for providing a buffer period for player next jump
        /// </summary>
        /// <param name="entityBody"></param>
        /// <param name="data"></param>
        public override void FrameEvent(EntityBody entityBody, AbilityData data) {
            TransformingPlayerData pmd = (TransformingPlayerData)data;
            pmd.queueJump = Mathf.Clamp(Time.deltaTime, 0, 0.2f);
        }

        /// <summary>
        /// Base logic for ai/player movement
        /// </summary>
        /// <param name="entityBody"></param>
        /// <param name="data"></param>
        /// <param name="inpVals"></param>
        /// <returns></returns>
        public override bool NormalMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {

            /// Prepare movement data
            TransformingPlayerData moveData = (TransformingPlayerData)data;
            moveData.isJumpingButtonPressed = inpVals.Direction.y > 0;
            moveData.chargeDirection = inpVals.Direction;
            float maxSpeed = moveData.isJumpingButtonPressed ? glideHorizontalSpeed : inpVals.IsRunning ? runSpeed : walkSpeed;

            /// Prepare for spider model on wall climb
            if (moveData.isJumpButtonRePressed) {
                CheckForWall(entityBody, entityBody.bodyHolder.transform.position, entityBody.bodyHolder.transform.forward, ref moveData.wallClimbObj, ref moveData.wallRaycastHits, wallCheckLayers, onClimb, moveData);
                moveData.velocity = Vector3.zero;
                moveData.fallSpeed = 0;
                moveData.isJumpButtonRePressed = false;
                moveData.isJumpingButtonPressed = true;
                moveData.releasedOnJump = false;
                entityBody.iAbility.OnMoveEntity(moveData.velocity);
                Debug.Log(moveData.velocity);
                return false;
            }

            /// Check for crouching - frog ability
            if (!moveData.isCrouching && inpVals.IsCrouching) {
                moveData.isCrouching = true;
                entityBody.iAbility.OnAbilityEvent(onCrouch);
            } else if (moveData.isCrouching && !inpVals.IsCrouching && entityBody.isGrounded) {
                moveData.isCrouching = false;
                entityBody.iAbility.OnAbilityEvent(onUncrouch);
            }

            /// Main entity movement
            moveData.velocity = AccelerationMovement(
                inpVals.Direction,
                inpVals.IsCrouching && moveData.isGrounded ? moveData.velocity * 0.3f : moveData.velocity,
                inpVals.IsCrouching && moveData.isGrounded ? maxSpeed * 0.3f : maxSpeed,
                inpVals.IsRunning,
                moveData.isGrounded
            );
            Gravity(moveData, entityBody);
            AnimateAbility(entityBody, moveData.velocity, moveData.fallSpeed, moveData.isGrounded);

            entityBody.iAbility.OnMoveEntity(moveData.velocity * Time.deltaTime);
            return true;
        }

        /// <summary>
        /// Charge movement used for the bear, has main directional drive with leeway for player input
        /// </summary>
        /// <param name="entityBody"></param>
        /// <param name="data"></param>
        /// <param name="inpVals"></param>
        /// <returns></returns>
        public override bool ChargeMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            TransformingPlayerData moveData = (TransformingPlayerData)data;
            if (moveData.chargeDirection == Vector3.zero) {
                moveData.chargeDirection = entityBody.animatorManager.transform.forward;
                moveData.chargeDirection.y = 0;
            }
            moveData.chargeDirection = Vector3.MoveTowards(moveData.chargeDirection, inpVals.inputAbilityValues.direction, chargeChangeDirectionAmount * Time.deltaTime);
            moveData.velocity = moveData.chargeDirection.normalized * chargeSpeed;
            Gravity(moveData, entityBody);
            entityBody.iAbility.OnMoveEntity(moveData.velocity * Time.deltaTime);
            return true;
        }

        /// <summary>
        /// For AI, send the AI directly to a location
        /// </summary>
        /// <param name="entityBody"></param>
        /// <param name="data"></param>
        /// <param name="inpVals"></param>
        /// <returns></returns>
        public override bool AutoTrackMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            entityBody.iAbility.OnEntityTrack(inpVals.Destination);
            AnimateAbility(entityBody, Vector3.forward * walkSpeed, 0, true);
            return true;
        }

        /// <summary>
        /// Old code, was supposed to be switchable to a crane but instead made a separate SO
        /// </summary>
        /// <param name="entityBody"></param>
        /// <param name="data"></param>
        /// <param name="inpVals"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool FlightMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputDir"> Player/AI input </param>
        /// <param name="currentVelocity"> current travel velocity </param>
        /// <param name="maxSpeed"> capped speed of entity </param>
        /// <param name="runInput"> whether entity is running </param>
        /// <param name="isGrounded"> whether entity is grounded </param>
        /// <returns></returns>
        private Vector3 AccelerationMovement(Vector3 inputDir, Vector3 currentVelocity, float maxSpeed, bool runInput, bool isGrounded) {
            Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);

            // If move input
            if (inputDir.sqrMagnitude > 0.001f) {
                inputDir.Normalize();

                // Acceleration calculations based on current travel direction
                Vector3 currentDir = horizontalVelocity.sqrMagnitude > 0.001f
                    ? horizontalVelocity.normalized
                    : inputDir;
                float directionMultiplier = Mathf.Lerp(
                    changeDirectionSpeedMultiplier,
                    1f,
                    Vector3.Dot(currentDir, inputDir)
                );
                float accel =
                    (isGrounded ?
                    acceleration :
                    accelerationWhileFalling) *
                    (runInput ? runAccelerationMultiplier : 1f) *
                    directionMultiplier * Time.deltaTime;

                horizontalVelocity += inputDir * accel;
                if (horizontalVelocity.magnitude > maxSpeed)
                    horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.ClampMagnitude(horizontalVelocity, maxSpeed), isGrounded ? deceleration : decelerationWhileFalling);
            }

            // If no Input
            else {
                horizontalVelocity = Vector3.MoveTowards(
                    horizontalVelocity,
                    Vector3.zero,
                    (isGrounded ? deceleration : decelerationWhileFalling) * Time.deltaTime
                );
            }

            return new Vector3(horizontalVelocity.x, 0, horizontalVelocity.z);
        }

        protected virtual void Gravity(TransformingPlayerData pmd, EntityBody entityBody) {

            // Grounded and jump logic
            bool hitGround = entityBody.isGrounded && pmd.fallSpeed <= 0;

            // Grounded conditions
            if (hitGround) {
                pmd.glideTime = 0;
                OnGrounded(entityBody, pmd);
            } else {
                pmd.isGrounded = hitGround;
                OnArial(entityBody, pmd);
            }

            //Set fallspeed
            pmd.fallSpeed = Mathf.Clamp(pmd.fallSpeed, -maxFallSpeed, jumpSpeed);
            pmd.velocity.y = pmd.fallSpeed;
        }

        /// <summary>
        /// Wallcheck using AreaColliderCheck to see whether a surface is climbable
        /// </summary>
        /// <param name="entityBody"> main body </param>
        /// <param name="position"> check from position </param>
        /// <param name="direction"> check in direction </param>
        /// <param name="raycastObj"> return value of closest surface </param>
        /// <param name="raycastHits"> return value of all surfaces </param>
        /// <param name="layerMask"> layers to check for in surfaces </param>
        /// <param name="climbEvent"> IAbility event on successful check </param>
        /// <param name="transformingPlayerData"> base player data </param>
        /// <returns></returns>
        public static bool CheckForWall(EntityBody entityBody, Vector3 position, Vector3 direction, ref RaycastHit raycastObj, ref RaycastHit[] raycastHits, LayerMask layerMask, string climbEvent, TransformingPlayerData transformingPlayerData = null) {
            int s = AreaColliderCheck.GetRayCastColliders(position, direction, layerMask).Invoke(raycastHits);
            if (s > 0) {
                raycastObj = raycastHits[0];
                if (transformingPlayerData != null) transformingPlayerData.velocity = Vector3.zero;
                entityBody.iAbility.OnAbilityEvent(climbEvent);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Jump options and IAbility event on grounded
        /// </summary>
        /// <param name="entityBody"></param>
        /// <param name="pmd"></param>
        private void OnGrounded(EntityBody entityBody, TransformingPlayerData pmd) {
            if (!pmd.isGrounded) {
                entityBody.iAbility.OnAbilityEvent(onHitGround);
                pmd.isGrounded = true;
            }
            // Grounded reset
            pmd.remainingJumps = doubleJumpCount;
            pmd.glideDeltaActivate = 0;
            pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, -groundedFallSpeed, decelerationWhileGrounded * Time.deltaTime);
            pmd.isJumpButtonRePressed = false;
            pmd.glideTime = 0;
            pmd.releasedOnJump = false;
            if (pmd.isJumpingButtonPressed && !pmd.isHoldingInput) {
                OnJump(pmd);
                pmd.isHoldingInput = true;
            } else pmd.isHoldingInput = false;
        }

        /// <summary>
        /// Non-grounded movement logic
        /// </summary>
        /// <param name="entityBody"></param>
        /// <param name="pmd"></param>
        private void OnArial(EntityBody entityBody, TransformingPlayerData pmd) {

            /// Arial event on jump button
            if (pmd.isJumpingButtonPressed)
                JumpInputArial(entityBody, pmd);

            /// On free-fall
            else {
                pmd.releasedOnJump = true;
                if (pmd.isJumpButtonRePressed) {
                    pmd.isJumpButtonRePressed = false;
                    entityBody.iAbility.OnAbilityEvent(onFreeFall);
                }
                pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, -maxFallSpeed, gravity * Time.deltaTime);
            }
        }

        private void JumpInputArial(EntityBody entityBody, TransformingPlayerData pmd) {
            // If glide is activated
            if (pmd.releasedOnJump) OnJump(pmd);
            if (pmd.canGlide && pmd.glideTime < maxGlideTime) {
                GlideEvent(entityBody, pmd, canGlide);
            } else {
                pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, -maxFallSpeed, gravity * Time.deltaTime);
            }
        }

        private void GlideEvent(EntityBody entityBody, TransformingPlayerData pmd, bool canGlide) {
            if (!pmd.isJumpButtonRePressed) {
                entityBody.iAbility.OnAbilityEvent(onGlide);
                pmd.isJumpButtonRePressed = true;
            }

            if (canGlide) {
                pmd.glideTime += Time.deltaTime;
                pmd.fallSpeed = Mathf.MoveTowards(
                    pmd.fallSpeed,
                    Mathf.Lerp(-maxFallSpeed, -maxGlideTime, Mathf.Clamp01(glideFallSpeedOverTime.Evaluate(pmd.glideTime / maxGlideTime))),
                    gravity * Time.deltaTime * (pmd.fallSpeed > -glideFallSpeed ? 1 : fallToGlideAcceleration)
                );
            } else {
                pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, -maxFallSpeed, gravity * Time.deltaTime);
            }
        }
        protected virtual void OnJump(TransformingPlayerData pmd) {
            if (!(canJump && pmd.canJump)) return;

            pmd.fallSpeed = jumpSpeed * 100;
            pmd.isGrounded = false;
            pmd.remainingJumps--;
        }

        protected void AnimateAbility(EntityBody entityBody, Vector3 movement, float fallSpeed, bool isGrounded) {
            float delta = Mathf.Clamp01(new Vector3(movement.x, 0, movement.z).magnitude / runSpeed);

            if (DEBUG) Debug.Log(movement);
            entityBody.
            animatorManager?.
            SetMovement(
                delta,
                Mathf.Lerp(
                    maxFallSpeed,
                    -maxFallSpeed,
                    fallSpeed),
                isGrounded
            );
        }

        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            throw new System.NotImplementedException();
        }
        #endregion
    }
    public class TransformingPlayerData : AbilityData {
        //Velocity Values
        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public float fallSpeed;
        [HideInInspector] public Vector3 chargeDirection;

        //Jumping
        [HideInInspector] public bool isJumpingButtonPressed;
        [HideInInspector] public bool releasedOnJump;
        [HideInInspector] public int remainingJumps;
        [HideInInspector] public float queueJump;//!!!!!!!!!!!!!
        [HideInInspector] public float glideTime;

        //States
        [HideInInspector] public bool isGrounded;
        [HideInInspector] public bool isRunning;
        [HideInInspector] public bool isCrouching;
        [HideInInspector] public bool isClimbing;

        [HideInInspector] public bool isJumpButtonRePressed;

        [HideInInspector] public float glideDeltaActivate = 0;
        [HideInInspector] public bool canGlide { get => isJumpingButtonPressed && fallSpeed < 0 && releasedOnJump; }
        [HideInInspector] public RaycastHit[] wallRaycastHits = new RaycastHit[1];
        [HideInInspector] public RaycastHit wallClimbObj;

        public bool canJump {
            get => !releasedOnJump && (isGrounded || remainingJumps > 0);
        }

        public TransformingPlayerData() {
            velocity = Vector3.zero;
            fallSpeed = 0;
            isGrounded = false;
            isRunning = false;
            releasedOnJump = false;
            remainingJumps = 0;
        }
    }
}