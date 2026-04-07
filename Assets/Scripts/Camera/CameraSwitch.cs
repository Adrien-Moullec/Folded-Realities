using UnityEngine;

public class CameraSwitch : MonoBehaviour {
    [Header("Camera Target")]
    [SerializeField] Vector3 targetLocalOffset = new Vector3(0, 3, -10);
    [SerializeField] Vector3 targetRotation = new Vector3(10, 0, 0);
    [SerializeField] float targetZoom = 5f;

    [Header("Transition")]
    [SerializeField] float moveSpeed = 5f;

    Camera cam;
    CameraFocus focusScript;

    bool playerInside;
    bool returning;

    Vector3 originalLocalPos;
    Quaternion originalRotation;
    float originalZoom;

    void Start() {
        cam = Camera.main;
        focusScript = cam.GetComponent<CameraFocus>();
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        playerInside = true;
        returning = false;

        
        originalLocalPos = cam.transform.localPosition;
        originalRotation = cam.transform.localRotation;
        originalZoom = cam.orthographicSize;

        if (focusScript != null) {
            focusScript.enabled = false;
        }
    }

    void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        playerInside = false;
        returning = true;
    }

    void LateUpdate() {
        if (cam == null) {
            return;
        }

        if (playerInside) {
            cam.transform.localPosition = Vector3.Lerp(
                cam.transform.localPosition,
                targetLocalOffset,
                Time.deltaTime * moveSpeed
            );

            cam.transform.localRotation = Quaternion.Lerp(
                cam.transform.localRotation,
                Quaternion.Euler(targetRotation),
                Time.deltaTime * moveSpeed
            );

            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                targetZoom,
                Time.deltaTime * moveSpeed
            );
        }

        else if (returning) {
            cam.transform.localPosition = Vector3.Lerp(
                cam.transform.localPosition,
                originalLocalPos,
                Time.deltaTime * moveSpeed
            );

            cam.transform.localRotation = Quaternion.Lerp(
                cam.transform.localRotation,
                originalRotation,
                Time.deltaTime * moveSpeed
            );

            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                originalZoom,
                Time.deltaTime * moveSpeed
            );

            if (Vector3.Distance(cam.transform.localPosition, originalLocalPos) < 0.05f) {
                returning = false;

                if (focusScript != null) {
                    focusScript.enabled = true;
                }
            }
        }
    }
}