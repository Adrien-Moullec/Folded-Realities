using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "GeneralMovement", menuName = "Origami/Movement/General Movement", order = -1)]
    public class GenericMovement : MovementSO {
        [Header("Animations")]
        [Tooltip("Idle animation information when model is still.")]
        [SerializeField] AbilityAnimation idleAnimation;

        [Tooltip("Walk animation information when model is walking.")]
        [SerializeField] AbilityAnimation walkingAnimation;

        [Tooltip("Run animation information when model is running.")]
        [SerializeField] AbilityAnimation runningAnimation;

        [Tooltip("Jump animation information when model is jumping.")]
        [SerializeField] AbilityAnimation jumpingAnimation;

        [Tooltip("Fall animation information when model is falling.")]
        [SerializeField] AbilityAnimation fallingAnimation;

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

        [Tooltip("Turn speed of the entity.")]
        [SerializeField, Min(0.01f)] protected float turnSpeed = 10f;

        [Tooltip("Run speed multiplier.")]
        [SerializeField] protected float runSpeedMultiplier = 1.5f;

        [Header("Speed References")]

        protected static float baseSpeed = 5f;

        protected float walkSpeed => baseSpeed * speedMultiplier;
        protected float runSpeed => baseSpeed * speedMultiplier * runSpeedMultiplier;

        [Header("Vertical Settings")]
        [Tooltip("Controls if entity can jump.")]
        [SerializeField] protected bool canJump = true;
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

        #region Setup

        public override AbilityData AbilityDataSetup() {
            return new GenericMovementData();
        }

        public override (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup() {
            return new (AbilityAnimation, WrapMode)[]
            {
                (idleAnimation,WrapMode.Loop),
                (walkingAnimation,WrapMode.Loop),
                (runningAnimation,WrapMode.Loop)
            };
        }

        #endregion

        #region Movement Logic

        public override void Move(EntityBody entityBody, AbilityData data, Vector3 moveInput, bool runInput) {
            GenericMovementData moveData = (GenericMovementData)data;

            HorizontalSpeed(moveData, new Vector3(moveInput.x, 0, moveInput.z), runInput);
            FallSpeed(moveData, entityBody, moveInput.y > 0);

            AnimateAbility(moveData, entityBody.animationComponent);

            entityBody.iAbility.OnMoveEntity(moveData.velocity * Time.deltaTime, turnSpeed);
        }

        private void HorizontalSpeed(GenericMovementData moveData, Vector3 inputDir, bool runInput) {
            Vector3 horizontalVelocity = new Vector3(moveData.velocity.x, 0, moveData.velocity.z);

            if (inputDir.sqrMagnitude > 0.001f) {
                inputDir.Normalize();

                Vector3 currentDir = horizontalVelocity.sqrMagnitude > 0.001f
                    ? horizontalVelocity.normalized
                    : inputDir;

                float directionMultiplier = Mathf.Lerp(
                    changeDirectionSpeedMultiplier,
                    1f,
                    Vector3.Dot(currentDir, inputDir)
                );

                float accel =
                    (moveData.isGrounded ? acceleration : accelerationWhileFalling) *
                    (runInput ? runAccelerationMultiplier : 1f) *
                    directionMultiplier * 10;

                horizontalVelocity += inputDir * accel * Time.deltaTime;
                float maxSpeed = runInput ? runSpeed : walkSpeed;

                if (horizontalVelocity.magnitude > maxSpeed)
                    horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.ClampMagnitude(horizontalVelocity, maxSpeed), moveData.isGrounded ? deceleration : decelerationWhileFalling);
            } else {
                horizontalVelocity = Vector3.MoveTowards(
                    horizontalVelocity,
                    Vector3.zero,
                    (moveData.isGrounded ? deceleration : decelerationWhileFalling) * Time.deltaTime * 10
                );
            }

            moveData.velocity.x = horizontalVelocity.x;
            moveData.velocity.z = horizontalVelocity.z;
        }

        private void FallSpeed(GenericMovementData pmd, EntityBody entityBody, bool isJumping) {
            Vector3 feetPos = entityBody.feet.transform.position + entityBody.feet.center;

            pmd.isGrounded = Physics.CheckSphere(
                feetPos,
                entityBody.feet.radius,
                groundLayers
            ) && pmd.fallSpeed <= 0;

            if (pmd.isGrounded) {
                if (isJumping && canJump) {
                    pmd.fallSpeed = jumpSpeed * 100;
                    pmd.isGrounded = false;
                } else {
                    pmd.fallSpeed = 0;
                }
            } else {
                if (canGlide && isJumping) {
                    pmd.isGliding = true;
                    pmd.fallSpeed = Mathf.MoveTowards(
                        pmd.fallSpeed,
                        -glideFallSpeed,
                        gravity * Time.deltaTime * (pmd.fallSpeed > -glideFallSpeed ? 1 : fallToGlideAcceleration)
                    );
                } else if (canGlide && !isJumping) {
                    pmd.isGliding = false;
                }

                pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, -maxFallSpeed, gravity * Time.deltaTime);
            }

            pmd.fallSpeed = Mathf.Clamp(pmd.fallSpeed, -maxFallSpeed, maxFallSpeed);

            pmd.velocity.y = pmd.fallSpeed;
        }

        protected void AnimateAbility(GenericMovementData moveData, Animation anim) {
            float magnitudeDelta = new Vector3(moveData.velocity.x, 0, moveData.velocity.z).magnitude / runSpeed;
            float walkCutoff = walkSpeed / runSpeed;

            float x = Mathf.Clamp01(magnitudeDelta);

            float weightIdle = 0f;
            float weightWalk = 0f;
            float weightRun = 0f;

            if (x <= walkCutoff) {
                float t = x / walkCutoff;
                weightIdle = 1f - t;
                weightWalk = t;
            } else {
                float t = (x - walkCutoff) / (1f - walkCutoff);
                weightWalk = 1f - t;
                weightRun = t;
            }

            idleAnimation.Blend(anim, weightIdle);
            walkingAnimation.Blend(anim, weightWalk);
            runningAnimation.Blend(anim, weightRun);
        }

        #endregion

        public class GenericMovementData : AbilityData {
            [HideInInspector] public Vector3 velocity;
            [HideInInspector] public float fallSpeed;
            [HideInInspector] public bool isGrounded;
            [HideInInspector] public bool isRunning;
            [HideInInspector] public bool isGliding;

            public GenericMovementData() {
                velocity = Vector3.zero;
                fallSpeed = 0;
                isGrounded = false;
                isRunning = false;
            }
        }
    }
}