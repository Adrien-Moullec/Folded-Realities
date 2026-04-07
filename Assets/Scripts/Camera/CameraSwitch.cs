using UnityEngine;

public class CameraSwitch : MonoBehaviour {
    [Header("Camera Target")]
    [SerializeField] Vector3 targetLocalOffset = new Vector3(0, 3, -10);
    [SerializeField] float targetZoom = 5f;

    [Header("Transition")]
    [SerializeField] float moveSpeed = 5f;

    Camera cam;
    CameraFocus focusScript;

    bool playerInside;
    bool returning;

    Vector3 originalLocalPos;
    float originalZoom;

    void Start() {
        cam = Camera.main;

        focusScript = cam.GetComponent<CameraFocus>();

        originalLocalPos = cam.transform.localPosition;
        originalZoom = cam.orthographicSize;
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        playerInside = true;
        returning = false;

       
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

            Vector3 lookTarget = transform.position + Vector3.up * 1.5f;

            Quaternion lookRot = Quaternion.LookRotation(
                lookTarget - cam.transform.position
            );

            cam.transform.rotation = Quaternion.Lerp(
                cam.transform.rotation,
                lookRot,
                Time.deltaTime * moveSpeed
            );

            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                targetZoom,
                Time.deltaTime * moveSpeed
            );
        }

        //  RETURN TO NORMAL
        else if (returning) {
            cam.transform.localPosition = Vector3.Lerp(
                cam.transform.localPosition,
                originalLocalPos,
                Time.deltaTime * moveSpeed
            );

            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                originalZoom,
                Time.deltaTime * moveSpeed
            );

            // once back  re-enable CameraFocus
            if (Vector3.Distance(cam.transform.localPosition, originalLocalPos) < 0.05f) {
                returning = false;

                if (focusScript != null)
                    focusScript.enabled = true;
            }
        }
    }
}