using UnityEngine;


namespace AbilitySystem
{
    [CreateAssetMenu(fileName = "GeneralMovement", menuName = "Origami/Movement/General Movement", order = -1)]
    public class GenericMovement : MovementSO
    {

        [Header("Speed Settings")]
        internal static float baseSpeed = 0.01f;
        [SerializeField] protected float speedMultiplier = 1;
        protected float speed { get => baseSpeed * speedMultiplier; }
        [SerializeField, Min(0)] protected float acceleration = 1;
        [SerializeField, Min(0)] protected float deceleration = 0.1f;
        [SerializeField, Min(0)] protected float fallingDeceleration = 2f;

        [Header("Dash Settings")]
        [SerializeField] protected float dashSpeedMultiplier = 1.5f;
        [SerializeField] protected float dashAccelerationMultiplier = 1;

        [Header("Vertical Settings")]
        [SerializeField] protected bool canJump = false;
        [SerializeField, Min(0)] protected float jumpSpeed = 0.07f;
        [SerializeField, Min(0)] protected float gravity = 0.2f;
        [SerializeField, Min(0)] protected float maxFallSpeed = 10f;
        [SerializeField] protected LayerMask groundLayers;
        public override AbilityData AbilityDataSetup() => new GenericMovementData();
        public override AbilityAnimation[] AbilityAnimationsSetup()
        {
            throw new System.NotImplementedException();
        }

        protected float FallSpeed(AbilityData data, EntityBody entityBody, bool isJumping)
        {
            GenericMovementData pmd = (GenericMovementData)data;
            pmd.isGrounded =
                Physics.CheckSphere(
                    entityBody.feet.center + entityBody.feet.transform.position,
                    entityBody.feet.radius,
                    groundLayers)
                    && (pmd.fallSpeed <= 0);
            if (pmd.isGrounded)
                if (isJumping && canJump)
                    pmd.fallSpeed = jumpSpeed;
                else
                    pmd.fallSpeed = Mathf.MoveTowards(pmd.fallSpeed, 0, fallingDeceleration * Time.deltaTime);
            else
                pmd.fallSpeed += -gravity * Time.deltaTime;

            return pmd.fallSpeed;
        }

        internal override void Move(EntityBody entityBody, AbilityData data, Vector3 moveInput, bool dashInput)
        {
            GenericMovementData moveData = (GenericMovementData)data;
            Vector3 move = new Vector3(moveInput.x, 0, moveInput.z);

            //If move input
            if (move.magnitude > 0)
            {
                move = move.normalized * acceleration * (dashInput ? dashAccelerationMultiplier : 1) * Time.deltaTime;

                moveData.currentDirection.y = 0;
                moveData.currentDirection += new Vector3(move.x, 0, move.z);
                moveData.currentDirection = Vector3.ClampMagnitude(moveData.currentDirection, speed * (dashInput ? dashSpeedMultiplier : 1));
            }
            //If no move input
            else
            {
                if (moveData.decelerationDelta < 1f)
                    if ((moveData.currentDirection.magnitude - deceleration * Time.deltaTime) > 0)
                        moveData.currentDirection = Vector3.ClampMagnitude(moveData.currentDirection, moveData.currentDirection.magnitude - deceleration * Time.deltaTime);
                    else
                        moveData.currentDirection = Vector3.zero;

                else
                    moveData.currentDirection = Vector3.zero;
            }

            moveData.currentDirection.y = 0;
            if (moveData.currentDirection != Vector3.zero)
                entityBody.bodyHolder.transform.forward = moveData.currentDirection;
            FallSpeed(moveData, entityBody, moveInput.y == 1);
            moveData.currentDirection.y = Mathf.Clamp(moveData.fallSpeed, -maxFallSpeed, maxFallSpeed);

            entityBody.iAbility.OnMoveEntity(moveData.currentDirection);
        }

        public class GenericMovementData : AbilityData
        {
            [HideInInspector] internal Vector3 currentDirection;
            [HideInInspector] internal float fallSpeed = 0;
            [HideInInspector] internal float decelerationDelta = 0;
            [HideInInspector] internal bool isGrounded = false;
            [HideInInspector] internal bool isDashing = false;

            public GenericMovementData()
            {
                currentDirection = Vector3.zero;
                fallSpeed = 0;
                decelerationDelta = 0;
                isGrounded = false;
                isDashing = false;
            }
        }
    }
}