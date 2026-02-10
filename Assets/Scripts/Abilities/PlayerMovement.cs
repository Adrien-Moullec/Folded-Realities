using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Movement", menuName = "Origami/Movement/Player Movement", order = 0)]
public class PlayerMovement : MovementSO
{
    public override AbilityData Setup()
    {
        return new PlayerMovementData();
    }

    internal override float FallSpeed(AbilityController abCont, AbilityData data, bool isJumping)
    {
        PlayerMovementData pmd = (PlayerMovementData)data;
        pmd.isGrounded =
            Physics.CheckSphere(
                abCont.entity.feet.center + abCont.entity.feet.transform.position,
                abCont.entity.feet.radius,
                groundLayers)
                && (pmd.fallSpeed <= 0);
        pmd.fallSpeed += -gravity * Time.deltaTime;
        Debug.Log(pmd.isGrounded);

        if (pmd.isGrounded)
            if (isJumping)
                pmd.fallSpeed = jumpSpeed;
            else
                pmd.fallSpeed = 0;
        return pmd.fallSpeed;
    }

    internal override void Move(AbilityController abCont, AbilityData data, Vector3 moveInput, bool dashInput)
    {
        PlayerMovementData pmd = (PlayerMovementData)data;
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.z);

        //If move input
        if (move.magnitude > 0)
        {
            move = move.normalized * acceleration * (dashInput ? dashAccelerationMultiplier : 1) * Time.deltaTime;

            pmd.currentDirection.y = 0;
            pmd.currentDirection += new Vector3(move.x, 0, move.z);
            pmd.currentDirection = Vector3.ClampMagnitude(pmd.currentDirection, speed * (dashInput ? dashSpeedMultiplier : 1));
        }
        //If no move input
        else
        {
            if (pmd.decelerationDelta < 1f)
            {
                if ((pmd.currentDirection.magnitude - deceleration * Time.deltaTime) > 0)
                    pmd.currentDirection = Vector3.ClampMagnitude(pmd.currentDirection, pmd.currentDirection.magnitude - deceleration * Time.deltaTime);
                else
                    pmd.currentDirection = Vector3.zero;

            }
            else
            {
                pmd.currentDirection = Vector3.zero;
            }
        }

        pmd.currentDirection.y = 0;
        if (pmd.currentDirection != Vector3.zero)
        {
            abCont.entity.body.transform.forward = pmd.currentDirection;
        }

        FallSpeed(abCont, pmd, moveInput.y == 1);
        pmd.currentDirection.y = Mathf.Clamp(pmd.fallSpeed, -maxFallSpeed, maxFallSpeed);

        abCont.entity.controller.Move(pmd.currentDirection);
    }

    public class PlayerMovementData : AbilityData
    {
        [HideInInspector] internal Vector3 currentDirection;
        [HideInInspector] internal float fallSpeed = 0;
        [HideInInspector] internal float decelerationDelta = 0;
        [HideInInspector] internal bool isGrounded = false;
        [HideInInspector] internal bool isDashing = false;

        public PlayerMovementData()
        {
            currentDirection = Vector3.zero;
            fallSpeed = 0;
            decelerationDelta = 0;
            isGrounded = false;
            isDashing = false;
        }
    }
}
