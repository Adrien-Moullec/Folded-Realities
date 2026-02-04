using UnityEngine;

[CreateAssetMenu(fileName = "Player Movement", menuName = "Origami/Movement/Player Movement", order = 0)]
public class PlayerMovement : MovementSO
{
    internal override float FallSpeed(AbilityController abCont, bool isJumping)
    {
        abCont.isGrounded =
            Physics.CheckSphere(
                abCont.entity.feet.center + abCont.entity.feet.transform.position, 
                abCont.entity.feet.radius, 
                groundLayers) 
                && (abCont.fallSpeed <= 0);
        abCont.fallSpeed += -gravity * Time.deltaTime;
        Debug.Log(abCont.isGrounded);

        abCont.testCube.position = abCont.entity.feet.transform.position;
        if (abCont.isGrounded)
            if (isJumping)
                abCont.fallSpeed = jumpSpeed;
            else
                abCont.fallSpeed = 0;
        return abCont.fallSpeed;
    }

    internal override void Move(AbilityController abCont, Vector3 moveInput)
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.z);

        //If move input
        if (move.magnitude > 0) {
            move *= acceleration * Time.deltaTime;
            abCont.currentDirection.y = 0;
            abCont.currentDirection += new Vector3(move.x, 0, move.z);
            abCont.currentDirection = Vector3.ClampMagnitude(abCont.currentDirection, speed);
            abCont.decelerationDelta = 1 - (abCont.currentDirection.magnitude / speed);
        }
        //If no move input
        else
        {
            if (abCont.decelerationDelta < 1f)
            {
                abCont.decelerationDelta += deceleration * Time.deltaTime;
                abCont.currentDirection = Vector3.MoveTowards(abCont.currentDirection, Vector3.zero, abCont.decelerationDelta);
            }
            else
            {
                abCont.currentDirection = Vector3.zero;
            }
        }
        
        abCont.currentDirection.y = 0;
        if (abCont.currentDirection != Vector3.zero) {
            abCont.entity.body.transform.forward = abCont.currentDirection;
        }
        
        FallSpeed(abCont, moveInput.y == 1);
        abCont.currentDirection.y = Mathf.Clamp(abCont.fallSpeed, -maxFallSpeed, maxFallSpeed);

        abCont.entity.controller.Move(abCont.currentDirection);
    }
}
