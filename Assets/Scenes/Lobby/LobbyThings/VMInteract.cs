using UnityEngine;

public class VMInteract : MonoBehaviour {

    public GameObject shopUI;

    public GameObject playerVisuals;
    public MonoBehaviour playerController;

    public Transform hatContainer;   
    public GameObject crownHat;      

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || hasTriggered) return;

        hasTriggered = true;
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

        hasTriggered = false;
    }

    

    public void BuyCrown() {
        EquipHat(crownHat);
    }

    void EquipHat(GameObject hat) {
        foreach (Transform h in hatContainer) {
            h.gameObject.SetActive(false);
        }

        hat.SetActive(true);
    }
}