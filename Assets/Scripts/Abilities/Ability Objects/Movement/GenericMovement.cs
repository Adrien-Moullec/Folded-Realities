using UnityEngine;

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
        [Tooltip("Layers that count as ground.")]
        [SerializeField] protected string onHitGround;
        [SerializeField] protected string onGlide;
        [SerializeField] protected string onFreeFall;
        [SerializeField] protected string onHitWall;
        #region Setup

        public override AbilityData AbilityDataSetup(EntityBody eb) => new GenericMovementData();
        #endregion

        #region Movement Logic

        public override bool NormalMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            GenericMovementData moveData = (GenericMovementData)data;
            moveData.chargeDirection = inpVals.Direction;

            float maxSpeed = moveData.isGliding ? glideHorizontalSpeed : inpVals.IsRunning ? runSpeed : walkSpeed;

            moveData.velocity = AccelerationMovement(
                moveData,
                new Vector3(inpVals.Direction.x, 0, inpVals.Direction.z),
                maxSpeed,
                inpVals.IsRunning,
                inpVals.IsAccelerating
            );
            Gravity(moveData, entityBody, inpVals.Direction.y > 0);
            AnimateAbility(moveData, entityBody.animatorManager);

            entityBody.iAbility.OnMoveEntity(moveData.velocity * Time.deltaTime);
            return true;
        }
        public override bool ChargeMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            GenericMovementData moveData = (GenericMovementData)data;
            if (moveData.chargeDirection == Vector3.zero) {
                moveData.chargeDirection = entityBody.modelPrefab.transform.forward;
                moveData.chargeDirection.y = 0;
            }
            moveData.velocity = moveData.chargeDirection.normalized * chargeSpeed;
            Gravity(moveData, entityBody, false);
            entityBody.iAbility.OnMoveEntity(moveData.velocity * Time.deltaTime);
            return true;
        }
        public override bool AutoTrackMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }
        public override bool FlightMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }


        private Vector3 AccelerationMovement(GenericMovementData moveData, Vector3 inputDir, float maxSpeed, bool runInput, bool accelerate) {
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

        private void Gravity(GenericMovementData pmd, EntityBody entityBody, bool isJumping) {
            Vector3 feetPos = entityBody.feetSphereArea.transform.position + entityBody.feetSphereArea.center;

            pmd.isGrounded = Physics.CheckSphere(
                feetPos,
                entityBody.feetSphereArea.radius,
                groundLayers
            ) && pmd.fallSpeed <= 0.1f;
            if (!isJumping) pmd.performedJump = false;
            entityBody.iAbility.OnAbilityEvent(onHitGround);
            Collider[] colliders = Physics.OverlapSphere(feetPos, entityBody.feetSphereArea.radius, groundLayers);
            //foreach (var n in colliders) Debug.Log(n.gameObject.name);

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

        protected void AnimateAbility(GenericMovementData moveData, AnimatorManager anim) {
            float walkCutoff = walkSpeed / runSpeed;
            float delta = Mathf.Clamp01(new Vector3(moveData.velocity.x, 0, moveData.velocity.z).magnitude / runSpeed);

            float weight = 0;
            if (delta <= walkCutoff) weight = Mathf.InverseLerp(0, walkCutoff, delta) / 2;
            else weight = (Mathf.InverseLerp(walkCutoff, 1, delta) / 2) + 0.5f;
            float deltaFall = Mathf.Lerp(maxFallSpeed, -maxFallSpeed, moveData.fallSpeed);

            anim.SetMovement(delta, Mathf.Lerp(maxFallSpeed, -maxFallSpeed, moveData.fallSpeed), moveData.isGrounded);
        }

        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            throw new System.NotImplementedException();
        }


        #endregion

        public class GenericMovementData : AbilityData {
            [HideInInspector] public Vector3 velocity;
            [HideInInspector] public float fallSpeed;
            [HideInInspector] public Vector3 chargeDirection;

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