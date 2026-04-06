using System.Threading;

using AbilitySystem;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerAbilityController))]
public class PlayerManager : MonoBehaviour, ICamera {
    public static PlayerManager player;
    #region Variables
    [Space]
    [Header("Camera Settings")]
    [SerializeField] Camera gameplayCamera;
    [SerializeField] Transform cameraHolder;
    [SerializeField] Transform cameraHolderCentre;
    [SerializeField, Range(-100, 0)] float cameraTiltMin;
    [SerializeField, Range(0, 100)] float cameraTiltMax;
    [SerializeField, Min(5)] float lerpSpeed = 10;
    float camYTilt = 30;
    private Vector3 GetCameraPosition {
        get => camArea != null ?
        camArea.GetCameraPosition(gameplayCamera, cameraHolder.position) + camArea.transform.position : cameraHolder.position;
    }
    CameraArea camArea;
    private Vector3 camDir;

    [Space]
    [Header("Script Managers")]
    [SerializeField] IAbility iAbility;
    [SerializeField] RadialMenuManager _RadialMenuManager;
    private PlayerInput _PlayerInput;

    [Space]
    [Header("Inputs")]
    InputAction moveInput;
    Vector2 deltaMove;
    InputAction lookInput;
    Vector2 deltaLook;
    InputAction jumpInput;
    bool isJumping;
    InputAction runInput;
    bool isRunning;
    InputAction ability1Input;
    InputAction radialWheel;
    bool wheelActive = false;

    InputAction primaryAttackInput;
    bool holdPrimaryAttack;
    #endregion

    void Awake() {
        player = this;
        iAbility = GetComponent<IAbility>();
    }
    #region On Start
    private void OnEnable() {
        iAbility = GetComponent<PlayerAbilityController>();
        _PlayerInput = GetComponent<PlayerInput>();

        moveInput = _PlayerInput.actions["Move"];
        lookInput = _PlayerInput.actions["Look"];
        jumpInput = _PlayerInput.actions["Jump"];
        runInput = _PlayerInput.actions["Sprint"];
        primaryAttackInput = _PlayerInput.actions["PrimaryAttack"];
        radialWheel = _PlayerInput.actions["RadialMenu"];
        ability1Input = _PlayerInput.actions["Ability1"];

        lookInput.performed += input => deltaLook = input.ReadValue<Vector2>();
        lookInput.canceled += input => deltaLook = input.ReadValue<Vector2>();
        runInput.performed += input => iAbility.GetInputValues.isRunning = true;
        runInput.canceled += input => iAbility.GetInputValues.isRunning = false;
        radialWheel.performed += input => {
            _RadialMenuManager?.SetWheelActive(true);
            wheelActive = true;
        };
        radialWheel.canceled += input => {
            _RadialMenuManager?.SetWheelActive(false);
            wheelActive = false;
            iAbility.InputTransitionName(_RadialMenuManager?.OnSegmentClicked());
        };
        primaryAttackInput.performed += input => iAbility.GetInputValues.isPrimaryAttack = true;
        primaryAttackInput.canceled += input => iAbility.GetInputValues.isPrimaryAttack = false;
        ability1Input.performed += input => iAbility.GetInputValues.isPrimaryAbility = true;
        ability1Input.canceled += input => iAbility.GetInputValues.isPrimaryAbility = false;
    }
    void OnDisable() {
        lookInput.performed -= input => deltaLook = input.ReadValue<Vector2>();
        lookInput.canceled -= input => deltaLook = input.ReadValue<Vector2>();
        runInput.performed -= input => iAbility.GetInputValues.isRunning = true;
        runInput.canceled -= input => iAbility.GetInputValues.isRunning = false;
        radialWheel.performed -= input => {
            _RadialMenuManager?.SetWheelActive(true);
            wheelActive = true;
        };
        radialWheel.canceled -= input => {
            _RadialMenuManager?.SetWheelActive(false);
            wheelActive = false;
            iAbility.InputTransitionName(_RadialMenuManager?.OnSegmentClicked());
        };
        primaryAttackInput.performed -= input => iAbility.GetInputValues.isPrimaryAttack = true;
        primaryAttackInput.canceled -= input => iAbility.GetInputValues.isPrimaryAttack = false;
    }
    #endregion

    #region Update Functions
    private void Update() {
        if (gameplayCamera != null) CameraSettings();
        Movement();
    }

    #region Camera
    void Movement() {

        if (wheelActive) return;
        Vector2 m = moveInput.ReadValue<Vector2>();
        float j = jumpInput.ReadValue<float>();
        camDir = Camera.main.transform.right * m.x + Camera.main.transform.forward * m.y;
        camDir.y = 0;
        camDir.Normalize();
        camDir.y = j > 0.5f ? 1 : 0;
        iAbility.GetInputValues.SetDirection(camDir);
    }
    void Attack() {
        holdPrimaryAttack = true;
    }
    void CameraSettings() {
        camYTilt = Mathf.Clamp(camYTilt - deltaLook.y, cameraTiltMin, cameraTiltMax);
        cameraHolderCentre.eulerAngles = new Vector3(
            camYTilt,
            cameraHolderCentre.eulerAngles.y + deltaLook.x,
            cameraHolderCentre.eulerAngles.x);
        gameplayCamera.transform.position = Vector3.MoveTowards(
            gameplayCamera.transform.position,
            GetCameraPosition,
            lerpSpeed * Time.deltaTime
        );
        Vector3 target = GetCameraPosition;
        gameplayCamera.transform.position = Vector3.MoveTowards(
            gameplayCamera.transform.position,
            target,
            lerpSpeed * Time.deltaTime * Vector3.Distance(target, gameplayCamera.transform.position)
        );
        gameplayCamera.transform.forward = (transform.position - GetCameraPosition).normalized;
    }

    public void OnCameraAreaEnter(CameraArea cameraArea) {
        camArea = cameraArea;
    }

    public void OnCameraAreaExit() {
        camArea = null;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cameraHolder.transform.position, cameraHolderCentre.transform.position);
    }
    #endregion
    #endregion
}
