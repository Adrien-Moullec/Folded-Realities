/*using UnityEngine;
using System.Collections;

public class PulledAway : MonoBehaviour {
    public NPCDialogue npcDialogue;
    public CameraFocus cameraFocus;

    public GameObject helpUI; 

    public Transform targetPoint;
    public float speed = 3f;

    public float wobbleAmount = 15f;
    public float wobbleSpeed = 8f;

    public AudioSource audioSource;
    public AudioClip pullSound;

    public float helpDisplayTime = 5f;

    private bool isBeingPulled = false;
    private Vector3 startRotation;

 // Saves initial rotation for wobble effect
    void Start() {
        startRotation = transform.eulerAngles;
 // Starts pull sequence when dialogue ends
        if (npcDialogue != null) {
            npcDialogue.onDialogueFinished += StartPullSequence;
        }
 // Ensures help UI starts hidden
        if (helpUI != null) {
            helpUI.SetActive(false);
        }
    }

    void Update() {
        if (isBeingPulled && targetPoint != null) {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPoint.position,
                speed * Time.deltaTime
            );
  // Creates wobble rotation effect
            float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;

            transform.rotation = Quaternion.Euler(
                startRotation.x,
                startRotation.y,
                startRotation.z + wobble
            );
    // Disables object once destination is reached
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f) {
                gameObject.SetActive(false);
            }
        }
    }

    void StartPullSequence() {
        if (npcDialogue != null) {
            npcDialogue.preventAutoClose = true;
            npcDialogue.StopAllCoroutines();
            npcDialogue.enabled = false;

           
            if (npcDialogue.dialogueUI != null) {
                npcDialogue.dialogueUI.SetActive(false);
            }
        }

        if (cameraFocus != null) {
            cameraFocus.StopFocus();
        }
  // Handles pull effects and UI timing
        StartCoroutine(PullRoutine());
    }
    IEnumerator PullRoutine() {
        isBeingPulled = true;

        if (audioSource != null && pullSound != null) {
            audioSource.PlayOneShot(pullSound);
        }
     // Displays help UI
        if (helpUI != null) {
            helpUI.SetActive(true);
        }

        yield return new WaitForSeconds(helpDisplayTime);

        // Hides help UI after delay
        if (helpUI != null) {
            helpUI.SetActive(false);
        }
    }
}*/