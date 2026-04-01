using UnityEngine;
using System.Collections;
using TMPro;

public class PulledAway : MonoBehaviour {
    public NPCDialogue npcDialogue;
    public CameraFocus cameraFocus;

    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public GameObject continuePrompt;

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
        }
    }

    void StartPullSequence() {
        StartCoroutine(PullRoutine());
    }

    IEnumerator PullRoutine() {
        if (npcDialogue != null) {
            npcDialogue.preventAutoClose = true;
        }

        isBeingPulled = true;

        if (cameraFocus != null) {
            cameraFocus.FocusOn(transform);
        }

        if (audioSource != null && pullSound != null) {
            audioSource.PlayOneShot(pullSound);
        }

        if (dialogueUI != null) {
            dialogueUI.SetActive(true);
        }

        if (dialogueText != null) {
            dialogueText.text = "aaaahhh— help!!";
        }

        if (continuePrompt != null) {
            continuePrompt.SetActive(false);
        }

        yield return new WaitForSeconds(helpDisplayTime);

        if (cameraFocus != null) {
            cameraFocus.ResetToPlayer();
        }

        if (dialogueUI != null) {
            dialogueUI.SetActive(false);
        }
    }
}