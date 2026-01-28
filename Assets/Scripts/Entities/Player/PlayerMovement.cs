using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Body")]
    [SerializeField] GameObject feet;

    [Space]
    [Header("Stats")]
    [SerializeField] EntityStatsSO tempStats;

    [Space]
    [Header("Movement")]
    [SerializeField, Min(0)] float baseSpeed = 0.01f;
    private float maxSpeed
    {
        get => baseSpeed * (tempStats==null ? 1 : tempStats.maxSpeed);
    }
    [SerializeField, Min(0)] float acceleration = 1;
    [SerializeField, Min(0)] float deceleration = 0.8f;
    [SerializeField, Min(0)] float jumpSpeed = 0.07f;
    [SerializeField] bool isGrounded = false;
    [SerializeField, Min(0)] float gravity = 0.2f;
    [SerializeField] LayerMask groundLayers;
    private float fallSpeed;
    private float decelerationDelta = 0;

    [Space]
    [Header("Script Managers")]
    private PlayerInput _playerInput;
    private CharacterController _characterController;

    [Space]
    [Header("Inputs")]
    InputAction moveInput;
    Vector3 moveDir;
    Vector3 currentDirection = Vector3.zero;
    Vector2 deltaMove;
    InputAction lookInput;
    Vector2 look;
    Vector2 deltaLook;
    InputAction jumpInput;
    bool isJumping;
    #endregion

    private void OnEnable() {
        _playerInput = GetComponent<PlayerInput>();
        _characterController = GetComponent<CharacterController>();
        moveInput = _playerInput.actions["Move"];
        lookInput = _playerInput.actions["Look"];
        jumpInput = _playerInput.actions["Jump"];

        moveInput.performed += input => deltaMove = input.ReadValue<Vector2>();
        moveInput.canceled += input => deltaMove = input.ReadValue<Vector2>();
        lookInput.performed += input => deltaLook = input.ReadValue<Vector2>();
        lookInput.canceled += input => deltaLook = input.ReadValue<Vector2>();
        jumpInput.performed += input => isJumping = true;
        jumpInput.canceled += input => isJumping = false;
    }

    private void Update() {
        //Player movement   
        PlayerFall();
        PlayerMove();
    }

    void PlayerFall() {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 0.5f, 0), 1, groundLayers) && (fallSpeed <=0);
        fallSpeed += -gravity * Time.deltaTime * (tempStats==null?1:tempStats.fallMultiplier);
        if (isGrounded)
            if (isJumping)
                fallSpeed = jumpSpeed;
            else
                fallSpeed = 0;
    }

    void PlayerMove() {
        moveDir = (Camera.main.transform.right * deltaMove.x) + (Camera.main.transform.forward * deltaMove.y);
        moveDir.y = 0;

        //If move input
        if (moveDir.magnitude > 0) {
            moveDir.Normalize();
            moveDir *= acceleration * Time.deltaTime;

            currentDirection += moveDir;
            currentDirection = Vector3.ClampMagnitude(currentDirection, maxSpeed);
            decelerationDelta = 1 - (currentDirection.magnitude / maxSpeed); 
        }
        //If no move input
        else
        {
            if (decelerationDelta < 0.9f) {
                decelerationDelta += deceleration * Time.deltaTime;
                currentDirection = Vector3.MoveTowards(currentDirection, Vector3.zero, decelerationDelta);
            }
            else {
                currentDirection = Vector3.zero;
            }
        }
        currentDirection.y = fallSpeed;

        _characterController.Move(currentDirection);
    }
}
