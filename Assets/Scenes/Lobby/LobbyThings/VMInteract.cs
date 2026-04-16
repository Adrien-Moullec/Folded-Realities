using UnityEngine;

public class VMInteract : CinemachineInteract {
    public GameObject shopUI;
    public GameObject interactText;

    public GameObject playerVisuals;
    public MonoBehaviour playerController;

    public Camera mainCamera;
    public Transform cameraPoint;

    private void Awake() {
        interactText.SetActive(false);
    }

    public override void OnInteract() {
        SetCameraHighPriority();
        OpenShop();
    }
    public override void OnCancelInteract() {
        SetCameraDefaultPriority();
        CloseShop();
    }

    void OpenShop() {
        shopUI.SetActive(true);

        if (playerController != null) {
            playerController.enabled = false;
        }

        if (playerVisuals != null) {
            playerVisuals.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        interactText.SetActive(false);
    }

    public void CloseShop() {
        shopUI.SetActive(false);

        if (playerController != null) {
            playerController.enabled = true;
        }

        if (playerVisuals != null) {
            playerVisuals.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        OnCancelInteract();
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            interactText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            interactText.SetActive(false);
        }
    }
}