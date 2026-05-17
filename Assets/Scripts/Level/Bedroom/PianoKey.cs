using UnityEngine;
using System.Collections;

public class PianoKey : MonoBehaviour {

    public int keyID;

    public PianoKeyManager puzzleManager;

    public AudioSource audioSource;

    public AudioClip note;

    public Transform pivot;

    public MeshRenderer meshRenderer;

    public Material pressedMaterial;

    public float pressAngle = 4f;

    public float pressSpeed = 12f;

    public float holdTime = 0.05f;

    bool isPressed = false;

    bool activated = false;

    Quaternion startRot;

    Material originalMaterial;

    void Start() {

        if (pivot != null) {
            startRot =
                pivot.localRotation;
        }

        if (meshRenderer != null) {
            originalMaterial =
                meshRenderer.material;
        }
    }

    void OnTriggerEnter(
        Collider other
    ) {

        if (
            other.CompareTag("Player")
            && !isPressed
        ) {
            PlayKey();
        }
    }

    public void PlayKey() {

        if (
            audioSource != null
            && note != null
        ) {
            audioSource.PlayOneShot(
                note
            );
        }

        bool correct = false;

        if (
            !activated
            && puzzleManager != null
        ) {

            correct =
                puzzleManager.PressKey(
                    keyID
                );
        }

        if (
            correct
            && meshRenderer != null
            && pressedMaterial != null
        ) {

            activated = true;

            meshRenderer.material =
                pressedMaterial;
        }

        if (
            pivot != null
            && gameObject.activeInHierarchy
        ) {
            StartCoroutine(
                PressAnimation()
            );
        }
    }

    public void ResetKey() {

        activated = false;

        if (
            meshRenderer != null
            && originalMaterial != null
        ) {

            meshRenderer.material =
                originalMaterial;
        }
    }

    IEnumerator PressAnimation() {

        isPressed = true;

        Quaternion downRot =
            startRot
            * Quaternion.Euler(
                pressAngle,
                0f,
                0f
            );

        float t = 0f;

        while (t < 1f) {

            t +=
                Time.deltaTime
                * pressSpeed;

            pivot.localRotation =
                Quaternion.Lerp(
                    startRot,
                    downRot,
                    t
                );

            yield return null;
        }

        yield return new WaitForSeconds(
            holdTime
        );

        t = 0f;

        while (t < 1f) {

            t +=
                Time.deltaTime
                * pressSpeed;

            pivot.localRotation =
                Quaternion.Lerp(
                    downRot,
                    startRot,
                    t
                );

            yield return null;
        }

        pivot.localRotation =
            startRot;

        isPressed = false;
    }
}