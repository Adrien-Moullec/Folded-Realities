using UnityEngine;
using UnityEngine.InputSystem.Interactions;


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

        [Space]
        [Header("Speed Settings")]
        [Tooltip("Adjust the overall speed.")]
        [SerializeField] protected float speedMultiplier = 1;
        [Tooltip("If the entity is travelling a different direction to the input movement, this value controls how extra quickly the entity moves back on themselves.")]
        [SerializeField, Min(1)] protected float changeDirectionSpeedMultiplier = 3;
        [Tooltip("The acceleration of the entity from a stopped position to walk/run speed.")]
        [SerializeField, Min(0.01f)] protected float acceleration = 1;
        [Tooltip("The deceleration of the entity from a moving state to stopped.")]
        [SerializeField, Min(0.01f)] protected float deceleration = 0.1f;
        [Tooltip("The falling deceleration when the player reaches ground. This is so if the player hits an edge, the player could slip off and keep some momentum from the previous falling state.")]
        [SerializeField, Min(0.01f)] protected float decelerationWhileFalling = 2f;
        [Tooltip("How quickly the player turns around. Might become obselete later.")]
        [SerializeField, Min(0.01f)] protected float turnSpeed = 1;
        [Tooltip("How quickly the player runs in proportion to the walking speed.")]
        [SerializeField] protected float runSpeedMultiplier = 1.5f;
        [Tooltip("The acceleration multiplier of the player when running in proportion to normal acceleration.")]
        [SerializeField] protected float runAccelerationMultiplier = 1;

        [Header("Speed References")]
        protected static float baseSpeed = 0.01f;
        protected float walkSpeed { get => baseSpeed * speedMultiplier; }
        protected float runSpeed { get => baseSpeed * speedMultiplier * runSpeedMultiplier; }

        [Space]
        [Header("Vertical Settings")]
        [Tooltip("Controls if this entity can jump.")]
        [SerializeField] protected bool canJump = false;
        [Tooltip("Jump velocity when player jumps from grounded position.")]
        [SerializeField, Min(0)] protected float jumpSpeed = 0.07f;
        [Tooltip("Acceleration at all times towards the ground.")]
        [SerializeField, Min(0)] protected float gravity = 0.2f;
        [Tooltip("The fastest speed that the entity can fall.")]
        [SerializeField, Min(0)] protected float maxFallSpeed = 10f;
        [Tooltip("The layers that count as the ground for the entity. Objects that don't have these layers won't stop the player's downward trajectory, even if the player looks still on the surface.")]
        [SerializeField] protected LayerMask groundLayers;

        #region Setup
        public override AbilityData AbilityDataSetup() => new GenericMovementData();
        public override (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup() => new (AbilityAnimation, WrapMode)[]
        {
            (idleAnimation,WrapMode.Loop),
            (walkingAnimation,WrapMode.Loop),
            (runningAnimation,WrapMode.Loop)
        };
        #endregion

        #region Movement Logic
        internal override void Move(EntityBody entityBody, AbilityData data, Vector3 moveInput, bool runInput) {
            GenericMovementData moveData = (GenericMovementData)data;
            Vector3 move = new Vector3(moveInput.x, 0, moveInput.z);

            // If move input
            if (move.magnitude > 0) {
                move = move.normalized * acceleration *
                (runInput ? runAccelerationMultiplier : 1) *
                0.01f * Time.deltaTime;

                //Set current Velocity, and adjust change in velocity to be greater if currentDirection and move input are different directions
                moveData.currentDirection.y = 0;
                moveData.currentDirection += new Vector3(move.x, 0, move.z) * Mathf.Lerp(changeDirectionSpeedMultiplier, 1, Vector3.Dot(moveData.currentDirection.normalized, move.normalized));
                moveData.currentDirection = Vector3.ClampMagnitude(moveData.currentDirection, runInput ? runSpeed : walkSpeed);
            }
            // If no move input
            else {
                // If still decelerating
                if (moveData.currentDirection != Vector3.zero) {
                    moveData.currentDirection = Vector3.MoveTowards(moveData.currentDirection, Vector3.zero, (moveData.isGrounded ? deceleration : decelerationWhileFalling) * Time.deltaTime * 0.01f);
                }
            }

            FallSpeed(moveData, entityBody, moveInput.y == 1);
            AnimateAbility(moveData, entityBody.animationComponent);
            entityBody.iAbility.OnMoveEntity(moveData.currentDirection, turnSpeed);
        }

        protected float FallSpeed(AbilityData data, EntityBody entityBody, bool isJumping) {
            GenericMovementData pmd = (GenericMovementData)data;
            pmd.isGrounded = Physics.CheckSphere(
                entityBody.feet.center + entityBody.feet.transform.position,
                entityBody.feet.radius,
                groundLayers)
                && (pmd.fallSpeed <= 0);

            if (pmd.isGrounded)
                if (isJumping && canJump)
                    pmd.fallSpeed = jumpSpeed * 0.01f;
                else
                    pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, 0, decelerationWhileFalling * Time.deltaTime);
            else
                pmd.fallSpeed += -gravity * Time.deltaTime * 0.01f;

            pmd.fallSpeed = Mathf.Clamp(pmd.fallSpeed, -maxFallSpeed, maxFallSpeed);
            pmd.currentDirection.y = pmd.fallSpeed;
            return pmd.fallSpeed;
        }

        protected void AnimateAbility(GenericMovementData moveData, Animation anim) {
            float magnitudeDelta = moveData.currentDirection.magnitude / runSpeed;
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
            [HideInInspector] internal Vector3 currentDirection;
            [HideInInspector] internal float fallSpeed = 0;
            [HideInInspector] internal bool isGrounded = false;
            [HideInInspector] internal bool isRunning = false;

            public GenericMovementData() {
                currentDirection = Vector3.zero;
                fallSpeed = 0;
                isGrounded = false;
                isRunning = false;
            }
        }
    }
}