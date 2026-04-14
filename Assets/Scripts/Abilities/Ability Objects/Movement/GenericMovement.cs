using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "GeneralMovement", menuName = MenuAssetNames.MovementAbility + "/General Movement", order = -1)]
    public class GenericMovement : MovementSO {

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

        [Header("Vertical Settings")]
        [Tooltip("Controls if entity can jump.")]
        [SerializeField] protected bool canJump = true;
        [Tooltip("Controls if entity can jump.")]
        [SerializeField, Range(1, 5)] protected int doubleJumpCount = 1;
        [Tooltip("Jump velocity.")]
        [SerializeField, Min(0)] protected float jumpSpeed = 7f;
        [Tooltip("Max fall speed.")]
        [SerializeField, Min(0)] protected float maxFallSpeed = 25f;
        [Tooltip("Controls if entity can glide.")]
        [SerializeField] protected bool canGlide = false;
        [Tooltip("The horizontal speed while gliding.")]
        [SerializeField] protected float glideHorizontalSpeed = 1;
        [Tooltip("The fall speed of gliding.")]
        [SerializeField] protected float glideFallSpeed = 1;
        [Tooltip("The speed to decelerate into glide fallspeed.")]
        [SerializeField] protected float fallToGlideAcceleration = 2;
        [Tooltip("Gravity acceleration.")]
        [SerializeField, Min(0)] protected float gravity = 20f;
        [Tooltip("Layers that count as ground.")]
        [SerializeField] protected LayerMask groundLayers;

        [Space]
        [Header("Movement Events")]
        [SerializeField] protected bool DebugLog = false;
        [SerializeField] protected string onHitGround;
        [SerializeField] protected string onGlide;
        [SerializeField] protected string onFreeFall;
        [SerializeField] protected string onHitWall;
        #region Setup

        public override AbilityData AbilityDataSetup(EntityBody eb) => new GenericMovementData();
        #endregion

        #region Movement Logic
        public override void Startup(EntityBody entityBody, AbilityData data) {
            GenericMovementData pmd = (GenericMovementData)data;
            if (pmd.isGrounded) entityBody.iAbility.OnAbilityEvent(onHitGround);
            if (!pmd.isGrounded && !pmd.isGliding) entityBody.iAbility.OnAbilityEvent(onFreeFall);
        }
        public override void FrameEvent(AbilityData data) {
            GenericMovementData pmd = (GenericMovementData)data;
            pmd.queueJump = Mathf.Clamp(Time.deltaTime, 0, 0.2f);
        }
        public override bool NormalMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            GenericMovementData moveData = (GenericMovementData)data;
            moveData.isJumpingButtonPressed = inpVals.Direction.y > 0;
            moveData.chargeDirection = inpVals.Direction;

            float maxSpeed = moveData.isJumpingButtonPressed ? glideHorizontalSpeed : inpVals.IsRunning ? runSpeed : walkSpeed;

            moveData.velocity = AccelerationMovement(
                moveData,
                new Vector3(inpVals.Direction.x, 0, inpVals.Direction.z),
                maxSpeed,
                inpVals.IsRunning
            );
            Gravity(moveData, entityBody);
            AnimateAbility(entityBody, moveData);

            entityBody.iAbility.OnMoveEntity(moveData.velocity * Time.deltaTime);
            return true;
        }
        public override bool ChargeMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            GenericMovementData moveData = (GenericMovementData)data;
            if (moveData.chargeDirection == Vector3.zero) {
                moveData.chargeDirection = entityBody.animatorManager.transform.forward;
                moveData.chargeDirection.y = 0;
            }
            moveData.velocity = moveData.chargeDirection.normalized * chargeSpeed;
            Gravity(moveData, entityBody);
            entityBody.iAbility.OnMoveEntity(moveData.velocity * Time.deltaTime);
            return true;
        }
        public override bool AutoTrackMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            entityBody.iAbility.OnEntityTrack(inpVals.Destination);
            return true;
        }
        public override bool FlightMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }


        private Vector3 AccelerationMovement(GenericMovementData moveData, Vector3 inputDir, float maxSpeed, bool runInput) {
            Vector3 horizontalVelocity = new Vector3(moveData.velocity.x, 0, moveData.velocity.z);

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
                    (moveData.isGrounded ?
                    acceleration :
                    accelerationWhileFalling) *
                    (runInput ? runAccelerationMultiplier : 1f) *
                    directionMultiplier * 10 * Time.deltaTime;

                horizontalVelocity += inputDir * accel;
                if (horizontalVelocity.magnitude > maxSpeed)
                    horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.ClampMagnitude(horizontalVelocity, maxSpeed), moveData.isGrounded ? deceleration : decelerationWhileFalling);
            }

            // If no Input
            else {
                horizontalVelocity = Vector3.MoveTowards(
                    horizontalVelocity,
                    Vector3.zero,
                    (moveData.isGrounded ? deceleration : decelerationWhileFalling) * Time.deltaTime * 10
                );
            }

            return new Vector3(horizontalVelocity.x, 0, horizontalVelocity.z);
        }

        private void Gravity(GenericMovementData pmd, EntityBody entityBody) {

            // Grounded and jump logic
            Vector3 feetPos = entityBody.feetSphereArea.transform.position + entityBody.feetSphereArea.center;
            bool hitGround = Physics.CheckSphere(
                feetPos,
                entityBody.feetSphereArea.radius,
                groundLayers
            ) && pmd.fallSpeed <= 0.1f;
            if (hitGround && !pmd.isGrounded) {
                entityBody.iAbility.OnAbilityEvent(onHitGround);
                pmd.isGrounded = true;
            } else {
                pmd.isGrounded = hitGround;
            }
            if (pmd.isJumpingButtonPressed && !pmd.hasAlreadyJumped) OnJump(pmd);
            if (!pmd.isJumpingButtonPressed) pmd.hasAlreadyJumped = false;

            if (DebugLog) {
                Collider[] colliders = Physics.OverlapSphere(feetPos, entityBody.feetSphereArea.radius, groundLayers);
                foreach (var c in colliders) Debug.Log(c.name);
            }

            // Grounded conditions
            if (hitGround)
                OnGrounded(pmd);
            else
                OnArial(entityBody, pmd);

            //Set fallspeed
            pmd.fallSpeed = Mathf.Clamp(pmd.fallSpeed, -maxFallSpeed, maxFallSpeed);
            pmd.velocity.y = pmd.fallSpeed;
        }
        private void OnGrounded(GenericMovementData pmd) {
            // Grounded reset
            pmd.remainingJumps = doubleJumpCount;
            pmd.glideDeltaActivate = 0;
            pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, 0, decelerationWhileGrounded * Time.deltaTime);
            pmd.isGliding = false;
        }
        private void OnArial(EntityBody entityBody, GenericMovementData pmd) {
            if (pmd.isJumpingButtonPressed)
                JumpInputArial(entityBody, pmd);
            else {
                pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, -maxFallSpeed, gravity * Time.deltaTime);
                entityBody.iAbility.OnAbilityEvent(onFreeFall);
            }
        }
        private void JumpInputArial(EntityBody entityBody, GenericMovementData pmd) {

            // If glide is activated
            if (canGlide && pmd.canGlide) {
                if (!pmd.isGliding) {
                    entityBody.iAbility.OnAbilityEvent(onGlide);
                    pmd.isGliding = true;
                }
                pmd.fallSpeed = Mathf.MoveTowards(
                    pmd.fallSpeed,
                    -glideFallSpeed,
                    gravity * Time.deltaTime * (pmd.fallSpeed > -glideFallSpeed ? 1 : fallToGlideAcceleration)
                );
            }

            //If Glide isn't activated
            else {
                if (pmd.isGliding) {
                    pmd.isGliding = false;
                    entityBody.iAbility.OnAbilityEvent(onFreeFall);
                }
                pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, -maxFallSpeed, gravity * Time.deltaTime);
            }
        }


        private void OnJump(GenericMovementData pmd) {
            if (!(canJump && pmd.canJump)) return;

            pmd.fallSpeed = jumpSpeed * 100;
            pmd.isGrounded = false;
            pmd.remainingJumps--;
            pmd.hasAlreadyJumped = true;
        }

        protected void AnimateAbility(EntityBody entityBody, GenericMovementData moveData) {
            float delta = Mathf.Clamp01(new Vector3(moveData.velocity.x, 0, moveData.velocity.z).magnitude / runSpeed);

            entityBody.animatorManager.SetMovement(delta, Mathf.Lerp(maxFallSpeed, -maxFallSpeed, moveData.fallSpeed), moveData.isGrounded);
        }

        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            throw new System.NotImplementedException();
        }


        #endregion

        public class GenericMovementData : AbilityData {
            //Velocity Values
            [HideInInspector] public Vector3 velocity;
            [HideInInspector] public float fallSpeed;
            [HideInInspector] public Vector3 chargeDirection;

            //Jumping
            [HideInInspector] public bool isJumpingButtonPressed;
            [HideInInspector] public bool hasAlreadyJumped;
            [HideInInspector] public int remainingJumps;
            [HideInInspector] public float queueJump;

            //States
            [HideInInspector] public bool isGrounded;
            [HideInInspector] public bool isRunning;
            [HideInInspector] public bool isGliding;
            [HideInInspector] public float glideDeltaActivate = 0;
            [HideInInspector] public bool canGlide { get => isJumpingButtonPressed && fallSpeed < 0; }

            public bool canJump {
                get => !hasAlreadyJumped && (isGrounded || remainingJumps > 0);
            }

            public GenericMovementData() {
                velocity = Vector3.zero;
                fallSpeed = 0;
                isGrounded = false;
                isRunning = false;
                hasAlreadyJumped = false;
                remainingJumps = 0;
            }
        }
    }
}