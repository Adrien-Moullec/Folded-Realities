using UnityEngine;

//public class VMInteract : CinemachineInteract {
public class VMInteract : MonoBehaviour {

    public GameObject shopUI;

    public GameObject playerVisuals;
    public MonoBehaviour playerController;

    private bool hasTriggered = false;

    public void OnInteract() {
        // not used anymore
    }

    public void OnCancelInteract() {
        //SetCameraDefaultPriority();
        CloseShop();
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || hasTriggered) return;

        hasTriggered = true;

        //SetCameraHighPriority();
        OpenShop();
    }

    void OpenShop() {
        if (shopUI != null)
            shopUI.SetActive(true);

        if (playerController != null)
            playerController.enabled = false;

        if (playerVisuals != null)
            playerVisuals.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseShop() {
        if (shopUI != null)
            shopUI.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        if (playerVisuals != null)
            playerVisuals.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //SetCameraDefaultPriority();
    }
}