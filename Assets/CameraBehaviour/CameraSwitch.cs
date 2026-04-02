using UnityEngine;

public class CameraSwitch : MonoBehaviour {
    [Header("Camera Target")]
    [SerializeField] Vector3 targetOffset = new Vector3(0, 3, -10);
    [SerializeField] Vector3 targetRotation = new Vector3(20, 0, 0);
    [SerializeField] float targetZoom = 5f;

    [Header("Transition")]
    [SerializeField] float moveSpeed = 5f;

    Camera cam;
    bool playerInside;

    Vector3 originalPos;
    Quaternion originalRot;
    float originalZoom;

    void Start() {
        cam = Camera.main;

        originalPos = cam.transform.position;
        originalRot = cam.transform.rotation;
        originalZoom = cam.orthographicSize;
    }

    void OnTriggerStay(Collider other) {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
    }

    void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
    }

    void LateUpdate() {
        if (cam == null) return;

        if (playerInside) {
            Vector3 targetPos = transform.position + targetOffset;

            cam.transform.position = Vector3.Lerp(
                cam.transform.position,
                targetPos,
                Time.deltaTime * moveSpeed
            );

            cam.transform.rotation = Quaternion.Lerp(
                cam.transform.rotation,
                Quaternion.Euler(targetRotation),
                Time.deltaTime * moveSpeed
            );

            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                targetZoom,
                Time.deltaTime * moveSpeed
            );
        } else {
            cam.transform.position = Vector3.Lerp(
                cam.transform.position,
                originalPos,
                Time.deltaTime * moveSpeed
            );

            cam.transform.rotation = Quaternion.Lerp(
                cam.transform.rotation,
                originalRot,
                Time.deltaTime * moveSpeed
            );

            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                originalZoom,
                Time.deltaTime * moveSpeed
            );
        }
    }
}