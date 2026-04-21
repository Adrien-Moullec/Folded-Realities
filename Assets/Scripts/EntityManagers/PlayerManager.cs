using System.Threading;

using AbilitySystem;

using Unity.Cinemachine;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerAbilityController))]
public class PlayerManager : MonoBehaviour {
    public static PlayerManager player;
    #region Variables
    [Space]
    [Header("Camera Settings")]
    [SerializeField] Camera gameplayCamera;
    //private Vector3 GetCameraPosition {
    //get => camArea != null ?
    //camArea.GetCameraPosition(gameplayCamera, cameraHolder.position) + camArea.transform.position : cameraHolder.position;
    //}
    CameraArea camArea;
    private Vector3 camDir;

    [Space]
    [Header("Script Managers")]
    [SerializeField] IAbility iAbility;
    [SerializeField] RadialMenuManager _RadialMenuManager;
    [SerializeField] PlayerAbilityController playerAbilityController;
    private PlayerInput _PlayerInput;

    [Space]
    [Header("Interaction")]
    [SerializeField] GameObject body;
    [SerializeField] AreaColliderCheck interactionArea;

    [Space]
    [Header("Inputs")]
    InputAction moveInput;
    InputAction lookInput;
    Vector2 deltaLook;
    InputAction jumpInput;
    InputAction runInput;
    InputAction ability1Input;
    InputAction ability2Input;
    InputAction ability3Input;
    InputAction radialWheel;
    InputAction interact;
    InputAction switchAction;
    bool wheelActive = false;
    #endregion

    void Awake() {
        player = this;
        iAbility = GetComponent<IAbility>();
    }
    #region On Start
    private void OnEnable() {
        playerAbilityController = GetComponent<PlayerAbilityController>();
        iAbility = playerAbilityController;
        _PlayerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;

        moveInput = _PlayerInput.actions["Move"];
        lookInput = _PlayerInput.actions["Look"];
        jumpInput = _PlayerInput.actions["Jump"];
        runInput = _PlayerInput.actions["Sprint"];
        radialWheel = _PlayerInput.actions["RadialMenu"];
        ability1Input = _PlayerInput.actions["Ability1"];
        ability2Input = _PlayerInput.actions["Ability2"];
        ability3Input = _PlayerInput.actions["Ability3"];
        interact = _PlayerInput.actions["Interact"];
        switchAction = _PlayerInput.actions["QuickSwitch"];

        lookInput.performed += input => deltaLook = input.ReadValue<Vector2>();
        lookInput.canceled += input => deltaLook = input.ReadValue<Vector2>();
        runInput.performed += input => iAbility.GetInputValues.SetRunToggle(true);
        runInput.canceled += input => iAbility.GetInputValues.SetRunToggle(false);
        switchAction.performed += input => playerAbilityController.QuickSwitch();
        radialWheel.performed += input => {
            _RadialMenuManager?.SetWheelActive(true);
            wheelActive = true;
            Cursor.lockState = CursorLockMode.None;
        };
        radialWheel.canceled += input => {
            _RadialMenuManager?.SetWheelActive(false);
            wheelActive = false;
            iAbility.InputTransitionName(_RadialMenuManager?.OnSegmentClicked());
            Cursor.lockState = CursorLockMode.Locked;
        };
        ability1Input.performed += input => iAbility.GetInputValues.isPrimaryAbility = true;
        ability1Input.canceled += input => iAbility.GetInputValues.isPrimaryAbility = false;
        ability2Input.performed += input => iAbility.GetInputValues.isSecondaryAbility = true;
        ability2Input.canceled += input => iAbility.GetInputValues.isSecondaryAbility = false;
        ability3Input.performed += input => iAbility.GetInputValues.isTertiaryAbility = true;
        ability3Input.canceled += input => iAbility.GetInputValues.isTertiaryAbility = false;
    }
    void OnDisable() {
        lookInput.performed -= input => deltaLook = input.ReadValue<Vector2>();
        lookInput.canceled -= input => deltaLook = input.ReadValue<Vector2>();
        runInput.performed -= input => iAbility.GetInputValues.SetRunToggle(true);
        runInput.canceled -= input => iAbility.GetInputValues.SetRunToggle(false);
        radialWheel.performed -= input => {
            _RadialMenuManager?.SetWheelActive(true);
            wheelActive = true;
        };
        radialWheel.canceled -= input => {
            _RadialMenuManager?.SetWheelActive(false);
            wheelActive = false;
            iAbility.InputTransitionName(_RadialMenuManager?.OnSegmentClicked());
        };
        ability1Input.performed -= input => iAbility.GetInputValues.isPrimaryAbility = true;
        ability1Input.canceled -= input => iAbility.GetInputValues.isPrimaryAbility = false;
        ability2Input.performed -= input => iAbility.GetInputValues.isSecondaryAbility = true;
        ability2Input.canceled -= input => iAbility.GetInputValues.isSecondaryAbility = false;
        ability3Input.performed -= input => iAbility.GetInputValues.isTertiaryAbility = true;
        ability3Input.canceled -= input => iAbility.GetInputValues.isTertiaryAbility = false;
    }
    #endregion

    void OnInteract() {
        Collider[] hits = new Collider[4];
        interactionArea.GetColliders(body).Invoke(hits);
        foreach (var n in hits)
            if (n.TryGetComponent(out IInteractable iinteract))
                iinteract.OnInteract();
    }

    #region Update Functions
    private void Update() {
        Movement();
    }

    #region Camera
    void Movement() {

        if (wheelActive) return;
        Vector2 m = moveInput.ReadValue<Vector2>();
        float j = jumpInput.ReadValue<float>();
        camDir = gameplayCamera.transform.right * m.x + gameplayCamera.transform.forward * m.y;
        camDir.y = 0;
        camDir.Normalize();
        camDir.y = j > 0.5f ? 1 : 0;
        iAbility.GetInputValues.SetDirection(camDir);
    }
    #endregion
    #endregion

    private void OnDrawGizmos() {
        interactionArea.Gizmo(body);
    }
}
