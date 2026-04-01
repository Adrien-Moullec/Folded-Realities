using UnityEngine;
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

    void Start() {
        startRotation = transform.eulerAngles;

        if (npcDialogue != null) {
            npcDialogue.onDialogueFinished += StartPullSequence;
        }

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

            float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;

            transform.rotation = Quaternion.Euler(
                startRotation.x,
                startRotation.y,
                startRotation.z + wobble
            );

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

        StartCoroutine(PullRoutine());
    }
    IEnumerator PullRoutine() {
        isBeingPulled = true;

        if (audioSource != null && pullSound != null) {
            audioSource.PlayOneShot(pullSound);
        }

        if (helpUI != null) {
            helpUI.SetActive(true);
        }

        yield return new WaitForSeconds(helpDisplayTime);

        if (helpUI != null) {
            helpUI.SetActive(false);
        }
    }
}