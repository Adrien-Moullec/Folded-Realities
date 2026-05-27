using UnityEngine;

public class TeleportTrigger : MonoBehaviour {

    [Header("Teleport")]
    [SerializeField] private Transform teleportPoint;

    [Header("Phases")]
    [SerializeField] private GameObject phase1;

    [SerializeField] private GameObject phase2;

    [Header("Camera")]
    [SerializeField] private Transform bossCamPoint;

    Camera mainCam;

    CameraFocus focusScript;

    void Start() {

        // Gets main camera and focus controller
        mainCam = Camera.main;

        if (mainCam != null)
            focusScript = mainCam.GetComponent<CameraFocus>();
    }

    void OnTriggerEnter(Collider other) {

        // Only activates for player
        if (!other.CompareTag("Player"))
            return;

        CharacterController controller =
            other.GetComponent<CharacterController>();

        // Temporarily disables controller for teleport
        if (controller != null)
            controller.enabled = false;

        other.transform.position = teleportPoint.position;

        if (controller != null)
            controller.enabled = true;

        // Switches phase objects
        if (phase1 != null)
            phase1.SetActive(false);

        if (phase2 != null)
            phase2.SetActive(true);

        // Disables camera follow system
        if (focusScript != null)
            focusScript.enabled = false;

        // Moves camera to boss position
        if (mainCam != null && bossCamPoint != null) {

            mainCam.transform.position = bossCamPoint.position;

            mainCam.transform.rotation = bossCamPoint.rotation;
        }

        // Disables debris effects
        GameObject[] debris =
            GameObject.FindGameObjectsWithTag("Debris");

        foreach (GameObject d in debris)
            d.SetActive(false);

        // Disables wind effects
        GameObject[] wind =
            GameObject.FindGameObjectsWithTag("Wind");

        foreach (GameObject w in wind)
            w.SetActive(false);
    }
}