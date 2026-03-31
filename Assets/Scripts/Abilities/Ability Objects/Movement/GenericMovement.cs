using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "GeneralMovement", menuName = MenuAssetNames.MovementAbility + "/General Movement", order = -1)]
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

        [Space]
        [Header("Speed References")]

        protected static float baseSpeed = 5f;

        protected float walkSpeed => baseSpeed * speedMultiplier;
        protected float runSpeed => baseSpeed * speedMultiplier * runSpeedMultiplier;

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
        [Tooltip("Layers that count as ground.")]
        [SerializeField] protected string onHitGround;
        [SerializeField] protected string onGlide;
        [SerializeField] protected string onFreeFall;
        [SerializeField] protected string onHitWall;
        #region Setup

        public override AbilityData AbilityDataSetup(EntityBody eb) {
            return new GenericMovementData();
        }
        public override (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup() {
            return new (AbilityAnimation, WrapMode)[]
            {
                (idleAnimation,WrapMode.Loop),
                (walkingAnimation,WrapMode.Loop),
                (runningAnimation,WrapMode.Loop),
                (jumpingAnimation,WrapMode.ClampForever)
            };
        }

        #endregion

        #region Movement Logic

        public override bool NormalMovement(EntityBody entityBody, AbilityData data) {
            GenericMovementData moveData = (GenericMovementData)data;
            AbilityInputValues inpVals = entityBody.iAbility.GetInputValues;

            float maxSpeed = moveData.isGliding ? glideHorizontalSpeed : inpVals.isRunning ? runSpeed : walkSpeed;
            moveData.velocity = HorizontalMovement(
                moveData,
                new Vector3(inpVals.inputDirection.x, 0, inpVals.inputDirection.z),
                maxSpeed,
                inpVals.isRunning, inpVals.isAccelerating
            );
            VerticalMovement(moveData, entityBody, inpVals.inputDirection.y > 0);
            AnimateAbility(moveData, entityBody.animationComponent);

            entityBody.iAbility.OnMoveEntity(moveData.velocity * Time.deltaTime);
            return true;
        }
        public override bool ChargeMovement(EntityBody entityBody, AbilityData data) {
            throw new System.NotImplementedException();
        }
        public override bool AutoTrackMovement(EntityBody entityBody, AbilityData data) {
            throw new System.NotImplementedException();
        }


        private Vector3 HorizontalMovement(GenericMovementData moveData, Vector3 inputDir, float maxSpeed, bool runInput, bool accelerate) {
            Vector3 horizontalVelocity = new Vector3(moveData.velocity.x, 0, moveData.velocity.z);

            // If move input
            if (inputDir.sqrMagnitude > 0.001f) {
                inputDir.Normalize();
                moveData.inputDirection = inputDir;


                // Accelerate if acceleration is turned on
                if (accelerate) {

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

                // Go to full speed
                else {
                    horizontalVelocity = inputDir * maxSpeed;
                }

            }

            // If no Input
            else {
                moveData.inputDirection =
                horizontalVelocity = Vector3.MoveTowards(
                    horizontalVelocity,
                    Vector3.zero,
                    (moveData.isGrounded ? deceleration : decelerationWhileFalling) * Time.deltaTime * 10
                );
            }

            return new Vector3(horizontalVelocity.x, 0, horizontalVelocity.z);
        }

        private void VerticalMovement(GenericMovementData pmd, EntityBody entityBody, bool isJumping) {
            Vector3 feetPos = entityBody.feetSphereArea.transform.position + entityBody.feetSphereArea.center;

            pmd.isGrounded = Physics.CheckSphere(
                feetPos,
                entityBody.feetSphereArea.radius,
                groundLayers
            ) && pmd.fallSpeed <= 0.1f;
            if (!isJumping) pmd.performedJump = false;
            entityBody.iAbility.OnAbilityEvent(onHitGround);
            /*Collider[] colliders = Physics.OverlapSphere(feetPos, entityBody.feetSphereArea.radius, groundLayers);
            foreach (var n in colliders)
                Debug.Log(n.gameObject.name);*/

            // Grounded conditions
            if (pmd.isGrounded) {

                // Grounded reset
                pmd.isGliding = false;
                pmd.remainingJumps = doubleJumpCount;

                // Jumping after just pressed
                if (pmd.canJump && isJumping)
                    OnJump(pmd);

                //Not jumping
                else {
                    pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, 0, decelerationWhileGrounded * Time.deltaTime);
                }

                // Arial Conditions
            } else {

                // When pressing jump input in the air
                if (isJumping) {

                    // When jump input has just been pressed
                    if (pmd.canJump)
                        OnJump(pmd);

                    // When jump input is being held
                    else {

                        // If glide is activated
                        if (canGlide && pmd.fallSpeed < 0) {
                            entityBody.iAbility.OnAbilityEvent(onGlide);
                            pmd.isGliding = true;
                            pmd.fallSpeed = Mathf.MoveTowards(
                                pmd.fallSpeed,
                                -glideFallSpeed,
                                gravity * Time.deltaTime * (pmd.fallSpeed > -glideFallSpeed ? 1 : fallToGlideAcceleration)
                            );
                        }

                        //If Glide isn't activated
                        else {
                            entityBody.iAbility.OnAbilityEvent(onGlide);
                            pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, -maxFallSpeed, gravity * Time.deltaTime);
                        }
                    }
                }
                // If falling
                else {
                    pmd.isGliding = false;
                    pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, -maxFallSpeed, gravity * Time.deltaTime);
                }
            }

            //Set fallspeed
            pmd.fallSpeed = Mathf.Clamp(pmd.fallSpeed, -maxFallSpeed, maxFallSpeed);
            pmd.velocity.y = pmd.fallSpeed;
        }

        private void OnJump(GenericMovementData pmd) {
            if (!canJump) return;
            pmd.fallSpeed = jumpSpeed * 100;
            pmd.isGrounded = false;
            pmd.remainingJumps--;
            pmd.performedJump = true;
        }

        protected void AnimateAbility(GenericMovementData moveData, Animation anim) {
            if (moveData.isGrounded) {
                float walkCutoff = walkSpeed / runSpeed;
                float x = Mathf.Clamp01(new Vector3(moveData.velocity.x, 0, moveData.velocity.z).magnitude / runSpeed);

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
                jumpingAnimation.SetWeight(anim, 0);
            } else {
                idleAnimation.Blend(anim, 0);
                walkingAnimation.Blend(anim, 0);
                runningAnimation.Blend(anim, 0);
                jumpingAnimation.PlayOnTimeline(anim, Mathf.InverseLerp(jumpSpeed, maxFallSpeed, moveData.fallSpeed));
                jumpingAnimation.SetWeight(anim, 1);
            }


        }

        #endregion

        public class GenericMovementData : AbilityData {
            [HideInInspector] public Vector3 velocity;
            [HideInInspector] public float fallSpeed;
            [HideInInspector] public Vector3 inputDirection;

            //Jumping
            [HideInInspector] public int remainingJumps;
            [HideInInspector] public bool performedJump;

            //States
            [HideInInspector] public bool isGrounded;
            [HideInInspector] public bool isRunning;
            [HideInInspector] public bool isGliding;

            public bool canJump {
                get => !performedJump && remainingJumps > 0;
            }

            public GenericMovementData() {
                velocity = Vector3.zero;
                fallSpeed = 0;
                isGrounded = false;
                isRunning = false;
                performedJump = false;
                remainingJumps = 0;
            }
        }
    }
}