using UnityEngine;
using System.Collections;

public class PianoKey : MonoBehaviour {

    public int keyID;
    public PianoKeyManager puzzleManager;

    public AudioSource audioSource;
    public AudioClip note;

    public GameObject highlight;
    public float flashDuration = 0.1f;

    private bool isPressed = false;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !isPressed) {
            PlayKey();
        }
    }

    public void PlayKey() {

        if (!gameObject.activeInHierarchy) {
            return;
        }

        if (audioSource != null && note != null) {
            audioSource.PlayOneShot(note);
        }

        if (puzzleManager != null) {
            puzzleManager.PressKey(keyID);
        }

        if (gameObject.activeInHierarchy) {
            StartCoroutine(PressEffect());
        }
    }

    IEnumerator PressEffect() {

        isPressed = true;

        if (highlight != null) {
            highlight.SetActive(true);
        }

        yield return new WaitForSeconds(flashDuration);

        if (highlight != null) {
            highlight.SetActive(false);
        }

        isPressed = false;
    }
}