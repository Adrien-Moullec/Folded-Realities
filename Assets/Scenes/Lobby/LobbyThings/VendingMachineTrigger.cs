using UnityEngine;

public class VendingMachineTrigger : MonoBehaviour
{
    public GameObject shopUI;
    public GameObject player;
    public MonoBehaviour playerController; // drag your movement script
    public Camera mainCamera;
    public Transform cameraPoint; // optional

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenShop();
        }
    }

    void OpenShop()
    {
        shopUI.SetActive(true);

        player.SetActive(false); // hides player

        if (playerController != null)
            playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (cameraPoint != null)
        {
            mainCamera.transform.position = cameraPoint.position;
            mainCamera.transform.rotation = cameraPoint.rotation;
        }
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);

        player.SetActive(true);

        if (playerController != null)
            playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}