using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerAbilityController))]
public class PlayerManager : MonoBehaviour, ICamera
{
    #region Variables
    [Space]
    [Header("Camera Settings")]
    [SerializeField] GameplayCamera gameplayCamera;
    [SerializeField] Transform cameraHolder;
    [SerializeField] Transform cameraHolderCentre;
    [SerializeField, Range(-100,0)] float cameraTiltMin;
    [SerializeField, Range(0,100)] float cameraTiltMax;
    float camYTilt = 30;
    float lerpSpeed = 100;
    private Vector3 GetCameraPosition
    {
        get => camArea != null ? camArea.cameraLocation + camArea.transform.position : cameraHolder.position;
    }
    CameraArea camArea;
    private Vector3 camDir;

    [Space]
    [Header("Script Managers")]
    [SerializeField] PlayerAbilityController AbilityController;
    private PlayerInput _playerInput;

    [Space]
    [Header("Inputs")]
    InputAction moveInput;
    Vector2 deltaMove;
    InputAction lookInput;
    Vector2 deltaLook;
    InputAction jumpInput;
    bool isJumping;
    InputAction dashInput;
    bool isDashing;
    #endregion

    #region On Start
    private void OnEnable()
    {
        AbilityController = GetComponent<PlayerAbilityController>();
        _playerInput = GetComponent<PlayerInput>();

        moveInput = _playerInput.actions["Move"];
        lookInput = _playerInput.actions["Look"];
        jumpInput = _playerInput.actions["Jump"];
        dashInput = _playerInput.actions["Sprint"];

        moveInput.performed += input => deltaMove = input.ReadValue<Vector2>();
        moveInput.canceled  += input => deltaMove = input.ReadValue<Vector2>();
        lookInput.performed += input => deltaLook = input.ReadValue<Vector2>();
        lookInput.canceled  += input => deltaLook = input.ReadValue<Vector2>();
        jumpInput.performed += input => isJumping = true;
        jumpInput.canceled  += input => isJumping = false;
        dashInput.performed += input => isDashing = true;
        dashInput.canceled  += input => isDashing = false;
    }
    void OnDisable()
    {
        moveInput.performed -= input => deltaMove = input.ReadValue<Vector2>();
        moveInput.canceled  -= input => deltaMove = input.ReadValue<Vector2>();
        lookInput.performed -= input => deltaLook = input.ReadValue<Vector2>();
        lookInput.canceled  -= input => deltaLook = input.ReadValue<Vector2>();
        jumpInput.performed -= input => isJumping = true;
        jumpInput.canceled  -= input => isJumping = false;
        dashInput.performed -= input => isDashing = true;
        dashInput.canceled  -= input => isDashing = false;
    }
    #endregion

    #region Update Functions
    private void Update()
    {
        camDir = Camera.main.transform.right * deltaMove.x + Camera.main.transform.forward * deltaMove.y;
        AbilityController.Move(new Vector3(camDir.x, isJumping ? 1 : 0, camDir.z), isDashing);
        CameraSettings();
    }

    #region Camera
    void CameraSettings()
    {
        camYTilt = Mathf.Clamp(camYTilt - deltaLook.y,cameraTiltMin,cameraTiltMax);
        cameraHolderCentre.eulerAngles = new Vector3(
            camYTilt, 
            cameraHolderCentre.eulerAngles.y + deltaLook.x, 
            cameraHolderCentre.eulerAngles.x);
        gameplayCamera.transform.position = Vector3.MoveTowards(
            gameplayCamera.transform.position,
            GetCameraPosition,
            100 * Time.deltaTime
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
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cameraHolder.transform.position,cameraHolderCentre.transform.position);
    }
    #endregion
    #endregion
}
