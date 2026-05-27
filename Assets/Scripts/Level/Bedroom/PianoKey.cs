using UnityEngine;
using System.Collections;

public class PianoKey : MonoBehaviour {

    #region References

    public int keyID;

    public PianoKeyManager puzzleManager;

    public AudioSource audioSource;

    public AudioClip note;

    #endregion

    #region Movement

    [Header("Movement")]
    public Transform pivot;

    public MeshRenderer meshRenderer;

    [Header("Materials")]
    public Material whiteMaterial;

    public Material colouredMaterial;

    public float pressAngle = 4f;

    public float pressSpeed = 12f;

    public float holdTime = 0.05f;

    #endregion

    #region Variables

    bool isPressed = false;

    bool activated = false;

    Quaternion startRot;

    #endregion

    void Start() {

        // Uses object transform if no pivot assigned
        if (pivot == null)
            pivot = transform;

        if (pivot != null)
            startRot = pivot.localRotation;

        // Sets default key material
        if (meshRenderer != null && colouredMaterial != null)
            meshRenderer.material = colouredMaterial;
    }

    void OnTriggerEnter(Collider other) {

        // Activates piano key when player touches it
        if (other.CompareTag("Player") && !isPressed)
            PlayKey();
    }

    #region Key Logic

    public void PlayKey() {

        // Plays piano note sound
        if (audioSource != null && note != null)
            audioSource.PlayOneShot(note);

        bool correct = false;

        // Sends key input to puzzle manager
        if (puzzleManager != null)
            correct = puzzleManager.PressKey(keyID);

        if (correct)
            activated = true;

        // Starts press animation
        if (pivot != null && gameObject.activeInHierarchy)
            StartCoroutine(PressAnimation());
    }

    public void FlashWhite() {

        // Flashes white material briefly
        if (gameObject.activeInHierarchy)
            StartCoroutine(FlashWhiteRoutine());
    }

    IEnumerator FlashWhiteRoutine() {

        if (meshRenderer == null || whiteMaterial == null || colouredMaterial == null)
            yield break;

        meshRenderer.material = whiteMaterial;

        yield return new WaitForSeconds(0.25f);

        meshRenderer.material = colouredMaterial;
    }

    public void ResetKey() {

        activated = false;

        // Resets piano key material
        if (meshRenderer != null && colouredMaterial != null)
            meshRenderer.material = colouredMaterial;
    }

    #endregion

    #region Animation

    IEnumerator PressAnimation() {

        isPressed = true;

        Quaternion downRot =
            startRot * Quaternion.Euler(pressAngle, 0f, 0f);

        float t = 0f;

        // Presses key downward
        while (t < 1f) {

            t += Time.deltaTime * pressSpeed;

            pivot.localRotation = Quaternion.Lerp(startRot, downRot, t);

            yield return null;
        }

        yield return new WaitForSeconds(holdTime);

        t = 0f;

        // Returns key to original rotation
        while (t < 1f) {

            t += Time.deltaTime * pressSpeed;

            pivot.localRotation = Quaternion.Lerp(downRot, startRot, t);

            yield return null;
        }

        pivot.localRotation = startRot;

        isPressed = false;
    }

    #endregion
}