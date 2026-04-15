using UnityEngine;

public class VendingMachineInteract : MonoBehaviour {
    public GameObject shopUI;
    public GameObject interactText; // "Press X"

    private bool playerInRange = false;

    void Start() {
        interactText.SetActive(false);
    }

    void Update() {
        if (playerInRange && Input.GetKeyDown(KeyCode.X)) {
            shopUI.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            interactText.SetActive(false);
        }
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