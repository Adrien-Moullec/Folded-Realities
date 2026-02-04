using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : MonoBehaviour, ICamera
{
    [Space]
    [Header("Camera Settings")]
    [SerializeField] GameplayCamera gameplayCamera;
    [SerializeField] Transform cameraHolder;
    [SerializeField, Min(0.01f)] float lerpSpeed = 0.01f;
    private Vector3 GetCameraPosition
    {
        get => camArea != null ? camArea.cameraLocation + camArea.transform.position : cameraHolder.position;
    }
    CameraArea camArea;
    float deltaCameraLerp = 1;

    #region Variables
    [Space]
    [Header("Abilities")]
    [SerializeField] AbilityController Abilities;
    [SerializeField] Transform cameraHolderCentre;


    [Space]
    [Header("Script Managers")]
    private PlayerInput _playerInput;

    [Space]
    [Header("Inputs")]
    InputAction moveInput;
    Vector3 moveDir;
    Vector3 currentDirection = Vector3.zero;
    Vector2 deltaMove;
    InputAction lookInput;
    Vector2 deltaLook;
    InputAction jumpInput;
    bool isJumping;
    InputAction dashInput;
    bool isDashing;

    private Vector3 camDir;
    #endregion

    private void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();

        moveInput = _playerInput.actions["Move"];
        lookInput = _playerInput.actions["Look"];
        jumpInput = _playerInput.actions["Jump"];
        dashInput = _playerInput.actions["Sprint"];

        moveInput.performed += input => deltaMove = input.ReadValue<Vector2>();
        moveInput.canceled += input => deltaMove = input.ReadValue<Vector2>();
        lookInput.performed += input => deltaLook = input.ReadValue<Vector2>();
        lookInput.canceled += input => deltaLook = input.ReadValue<Vector2>();
        jumpInput.performed += input => isJumping = true;
        jumpInput.canceled += input => isJumping = false;
        dashInput.performed += input => isDashing = true;
        dashInput.canceled += input => isDashing = false;
    }

    void Awake()
    {
        Abilities.Setup(gameObject);
    }

    private void Update()
    {
        camDir = (Camera.main.transform.right * deltaMove.x + Camera.main.transform.forward * deltaMove.y).normalized;
        camDir.y = isJumping ? 1 : 0;
        Abilities.Move(camDir);
        cameraHolderCentre.eulerAngles = cameraHolderCentre.eulerAngles + new Vector3(0, deltaLook.x, 0);
        CameraSettings();
    }

    #region Camera
    void CameraSettings()
    {
        gameplayCamera.transform.position = Vector3.MoveTowards(
            gameplayCamera.transform.position,
            GetCameraPosition,
            lerpSpeed * Time.deltaTime
        );
        gameplayCamera.transform.forward = (transform.position - GetCameraPosition).normalized;
    }

    public void OnCameraAreaEnter(CameraArea cameraArea)
    {
        camArea = cameraArea;
    }

    public void OnCameraAreaExit()
    {
        camArea = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(cameraHolder.transform.position, Vector3.one * 0.1f);
    }
    #endregion
}
