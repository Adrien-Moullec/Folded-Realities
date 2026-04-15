using UnityEngine;

public class VMInteract : MonoBehaviour {
    public GameObject shopUI;
    public GameObject interactText;

    public GameObject playerVisuals;
    public MonoBehaviour playerController;

    public Camera mainCamera;
    public Transform cameraPoint;

    private Vector3 originalCamPos;
    private Quaternion originalCamRot;

    private bool playerInRange = false;

    void Start() {
        interactText.SetActive(false);
    }

    void Update() {
        if (playerInRange && Input.GetKeyDown(KeyCode.X)) {
            OpenShop();
        }
    }

    void OpenShop() {
        shopUI.SetActive(true);

        if (playerController != null) {
            playerController.enabled = false;
        }

        if (playerVisuals != null) {
            playerVisuals.SetActive(false);
        }

        originalCamPos = mainCamera.transform.position;
        originalCamRot = mainCamera.transform.rotation;

        mainCamera.transform.position = cameraPoint.position;
        mainCamera.transform.rotation = cameraPoint.rotation;

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

        mainCamera.transform.position = originalCamPos;
        mainCamera.transform.rotation = originalCamRot;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
            interactText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
            interactText.SetActive(false);
        }
    }
}