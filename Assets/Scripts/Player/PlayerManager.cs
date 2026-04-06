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
    InputAction dashInput;
    bool isDashing;
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
        dashInput = _PlayerInput.actions["Sprint"];
        primaryAttackInput = _PlayerInput.actions["PrimaryAttack"];
        radialWheel = _PlayerInput.actions["RadialMenu"];

        lookInput.performed += input => deltaLook = input.ReadValue<Vector2>();
        lookInput.canceled += input => deltaLook = input.ReadValue<Vector2>();
        dashInput.performed += input => iAbility.GetInputValues.isRunning = true;
        dashInput.canceled += input => iAbility.GetInputValues.isRunning = false;
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
    }
    void OnDisable() {
        lookInput.performed -= input => deltaLook = input.ReadValue<Vector2>();
        lookInput.canceled -= input => deltaLook = input.ReadValue<Vector2>();
        dashInput.performed -= input => iAbility.GetInputValues.isRunning = true;
        dashInput.canceled -= input => iAbility.GetInputValues.isRunning = false;
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
<<<<<<< HEAD
        iAbility.GetInputValues.SetDirection(new Vector3(m.x, j > 0.5f ? 1 : 0, m.y));
=======

        iAbility.GetInputValues.SetDirection(new Vector3(camDir.x, j > 0.5f ? 1 : 0, camDir.y));

        camDir.y = 0;
        camDir.Normalize();
        camDir.y = j > 0.5f ? 1 : 0;
        iAbility.GetInputValues.SetDirection(camDir); ///new Vector3(camDir.x, j > 0.5f ? 1 : 0, camDir.y));
        Debug.Log(camDir);

>>>>>>> d63a96348acdbf449fa586deb1149e871f18c258
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
