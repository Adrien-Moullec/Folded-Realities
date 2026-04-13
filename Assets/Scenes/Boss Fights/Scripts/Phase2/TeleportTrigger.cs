using UnityEngine;

public class TeleportTrigger : MonoBehaviour {
    [Header("Teleport")]
    [SerializeField] private Transform teleportPoint;

    [Header("Phases")]
    [SerializeField] private GameObject phase1;
    [SerializeField] private GameObject phase2;

    [Header("Camera")]
    [SerializeField] private Transform bossCamPoint;

    private Camera mainCam;
    private CameraFocus focusScript;

    private void Start() {
        mainCam = Camera.main;

        if (mainCam != null) {
            focusScript = mainCam.GetComponent<CameraFocus>();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        CharacterController controller = other.GetComponent<CharacterController>();

        if (controller != null) {
            controller.enabled = false;
        }

        other.transform.position = teleportPoint.position;

        if (controller != null) {
            controller.enabled = true;
        }

        if (phase1 != null) {
            phase1.SetActive(false);
        }

        if (phase2 != null) {
            phase2.SetActive(true);
        }

        if (focusScript != null) {
            focusScript.enabled = false;
        }

        if (mainCam != null && bossCamPoint != null) {
            mainCam.transform.position = bossCamPoint.position;
            mainCam.transform.rotation = bossCamPoint.rotation;
        }

        GameObject[] debris = GameObject.FindGameObjectsWithTag("Debris");
        foreach (GameObject d in debris) {
            d.SetActive(false);
        }

        GameObject[] wind = GameObject.FindGameObjectsWithTag("Wind");
        foreach (GameObject w in wind) {
            w.SetActive(false);
        }
    }
}