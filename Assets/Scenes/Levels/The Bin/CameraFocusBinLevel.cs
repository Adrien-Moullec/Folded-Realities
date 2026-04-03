using UnityEngine;

public class CameraFocusBinLevel : MonoBehaviour
{
   
    [SerializeField] Transform player;
    [SerializeField] Transform npc;
    [SerializeField] Camera cam;

    [Header("Zoom Settings")]
    [SerializeField] float targetOrthoSize = 3f;
    [SerializeField] float zoomSpeed = 3f;

    [Header("Follow Settings")]
    [SerializeField] float followSpeed = 5f;
    [SerializeField] float fixedZ = -10f;

    float originalSize;
    bool active;

    void Start() {
        if (cam == null) {
            cam = Camera.main;
        }

        originalSize = cam.orthographicSize;
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        active = true;
    }

    void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        active = false;
    }

    void LateUpdate() {
        if (cam == null || player == null) {
            return;
        }

        if (active) {
            Vector3 midpoint = (player.position + npc.position) / 2f;

            Vector3 targetPos = new Vector3(
                midpoint.x,
                midpoint.y,
                fixedZ
            );

            cam.transform.position = Vector3.Lerp(
                cam.transform.position,
                targetPos,
                Time.deltaTime * followSpeed
            );

            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                targetOrthoSize,
                Time.deltaTime * zoomSpeed
            );
        } else {
            Vector3 playerPos = new Vector3(
                player.position.x,
                player.position.y,
                fixedZ
            );

            cam.transform.position = Vector3.Lerp(
                cam.transform.position,
                playerPos,
                Time.deltaTime * followSpeed
            );

            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                originalSize,
                Time.deltaTime * zoomSpeed
            );
        }
    }
}