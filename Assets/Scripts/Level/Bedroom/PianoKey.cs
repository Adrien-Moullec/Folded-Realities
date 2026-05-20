using UnityEngine;
using System.Collections;

public class PianoKey : MonoBehaviour {

    public int keyID;

    public PianoKeyManager puzzleManager;

    public AudioSource audioSource;

    public AudioClip note;

    [Header("Movement")]
    public Transform pivot;

    public MeshRenderer meshRenderer;

    [Header("Materials")]
    public Material whiteMaterial;

    public Material colouredMaterial;

    public float pressAngle = 4f;

    public float pressSpeed = 12f;

    public float holdTime = 0.05f;

    bool isPressed = false;

    bool activated = false;

    Quaternion startRot;

    void Start() {

        if (
            pivot == null
        ) {
            pivot =
                transform;
        }

        if (
            pivot != null
        ) {
            startRot =
                pivot.localRotation;
        }

        if (
            meshRenderer != null
            && colouredMaterial != null
        ) {
            meshRenderer.material =
                colouredMaterial;
        }
    }

    void OnTriggerEnter(
        Collider other
    ) {

        if (
            other.CompareTag(
                "Player"
            )
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
            puzzleManager != null
        ) {

            correct =
                puzzleManager.PressKey(
                    keyID
                );
        }

        if (
            correct
        ) {
            activated = true;
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

    public void FlashWhite() {

        if (
            gameObject.activeInHierarchy
        ) {
            StartCoroutine(
                FlashWhiteRoutine()
            );
        }
    }

    IEnumerator FlashWhiteRoutine() {

        if (
            meshRenderer == null
            || whiteMaterial == null
            || colouredMaterial == null
        ) {
            yield break;
        }

        meshRenderer.material =
            whiteMaterial;

        yield return new WaitForSeconds(
            0.25f
        );

        meshRenderer.material =
            colouredMaterial;
    }

    public void ResetKey() {

        activated = false;

        if (
            meshRenderer != null
            && colouredMaterial != null
        ) {

            meshRenderer.material =
                colouredMaterial;
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

        while (
            t < 1f
        ) {

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

        while (
            t < 1f
        ) {

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