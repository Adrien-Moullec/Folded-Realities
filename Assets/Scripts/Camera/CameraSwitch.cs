using System.Collections;

using UnityEngine;

public class CameraSwitch : MonoBehaviour {
    public enum CameraMode { Position, Fixed, Security }

    [Header("Mode")]
    public CameraMode mode = CameraMode.Position;

    [Header("Main Camera")]
    [SerializeField] Camera mainCamera;

    [Header("Reference Camera (Editor Placement)")]
    [SerializeField] Transform cameraPoint;

    [Header("Transition")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Zoom")]
    [SerializeField] bool useZoom = false;
    [SerializeField] float targetZoom = 5f;
    [SerializeField] float zoomSpeed = 5f;

    [Header("Player Control")]
    [SerializeField] MonoBehaviour playerController;
    [SerializeField] bool freezePlayer = false;
    [SerializeField] float freezeTime = 2f;

    [Header("Security Cam")]
    [SerializeField] Transform lookTarget;
    [SerializeField] float lookSpeed = 5f;

    [Header("Ground Fix")]
    [SerializeField] float forceGroundSnap = -2f;

    CameraFocus focusScript;

    static CameraSwitch currentZone;

    bool active;
    bool returning;

    float originalZoom;

    void Start() {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }

        focusScript = mainCamera.GetComponent<CameraFocus>();
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        if (currentZone != null && currentZone != this) {
            currentZone.ForceExit();
        }

        currentZone = this;

        originalZoom = mainCamera.orthographicSize;

        active = true;
        returning = false;

        ForceGroundPlayer(other.gameObject);

        if (focusScript != null) {
            focusScript.enabled = false;
        }

        if (freezePlayer) {
            StartCoroutine(DisablePlayerTemp());
        }
    }

    IEnumerator DisablePlayerTemp() {
        if (playerController != null) {
            playerController.enabled = false;
        }

        yield return new WaitForSeconds(freezeTime);

        if (playerController != null) {
            playerController.enabled = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        active = false;
        returning = true;
    }

    public void ForceExit() {
        StopAllCoroutines();

        active = false;
        returning = true;

        if (playerController != null) {
            playerController.enabled = true;
        }
    }

    void ForceGroundPlayer(GameObject player) {
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null) {
            Vector3 downward = new Vector3(0f, forceGroundSnap, 0f);
            cc.Move(downward);
        }
    }

    void LateUpdate() {
        if (mode == CameraMode.Security) {
            if (active) {
                if (cameraPoint != null) {
                    mainCamera.transform.position = Vector3.Lerp(
                        mainCamera.transform.position,
                        cameraPoint.position,
                        Time.deltaTime * moveSpeed
                    );

                    if (lookTarget != null) {
                        Vector3 dir = lookTarget.position - mainCamera.transform.position;
                        Quaternion lookRot = Quaternion.LookRotation(dir);

                        mainCamera.transform.rotation = Quaternion.Lerp(
                            mainCamera.transform.rotation,
                            lookRot,
                            Time.deltaTime * lookSpeed
                        );
                    }
                }
            }
        }

        if (mode == CameraMode.Fixed) {
            if (active) {
                if (cameraPoint != null) {
                    mainCamera.transform.position = Vector3.Lerp(
                        mainCamera.transform.position,
                        cameraPoint.position,
                        Time.deltaTime * moveSpeed
                    );

                    mainCamera.transform.rotation = Quaternion.Lerp(
                        mainCamera.transform.rotation,
                        cameraPoint.rotation,
                        Time.deltaTime * moveSpeed
                    );
                }
            }
        }

        if (mode == CameraMode.Position) {
            if (active) {
                if (cameraPoint != null) {
                    mainCamera.transform.position = Vector3.Lerp(
                        mainCamera.transform.position,
                        cameraPoint.position,
                        Time.deltaTime * moveSpeed
                    );

                    mainCamera.transform.rotation = Quaternion.Lerp(
                        mainCamera.transform.rotation,
                        cameraPoint.rotation,
                        Time.deltaTime * moveSpeed
                    );
                }
            }
        }

        if (useZoom) {
            if (active) {
                mainCamera.orthographicSize = Mathf.Lerp(
                    mainCamera.orthographicSize,
                    targetZoom,
                    Time.deltaTime * zoomSpeed
                );
            }
        }

        if (returning) {
            if (useZoom) {
                mainCamera.orthographicSize = Mathf.Lerp(
                    mainCamera.orthographicSize,
                    originalZoom,
                    Time.deltaTime * zoomSpeed
                );
            }

            if (focusScript != null) {
                focusScript.enabled = true;
            }

            returning = false;
        }
    }
}