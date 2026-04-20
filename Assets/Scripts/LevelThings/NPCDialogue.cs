using System.Collections;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour {
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public GameObject continuePrompt;

    [TextArea(2, 5)]
    public string[] lines;
    public float typingSpeed = 0.03f;

    public float popSpeed = 6f;
    public float popScale = 1.2f;

    public AudioSource audioSource;
    public AudioClip popSound;
    public AudioClip[] speechSounds;
    public float minPitch = 0.8f;
    public float maxPitch = 1.0f;

    public System.Action onDialogueFinished;
    public bool preventAutoClose = false;

    private int currentLine = 0;
    private bool playerNearby = false;
    private bool isTyping = false;
    private bool dialogueActive = false;
    private Vector3 originalScale;

    public CameraFocus cameraFocus;
    public CameraZoom cameraZoom;

    void Start() {
        if (dialogueUI != null) {
            dialogueUI.SetActive(false);
        }

        // FIX: ensure scale isn't broken
        originalScale = Vector3.one;
        dialogueUI.transform.localScale = Vector3.zero;

        if (continuePrompt != null) {
            continuePrompt.SetActive(false);
        }

        if (cameraFocus == null) {
            cameraFocus = FindAnyObjectByType<CameraFocus>();
        }

        if (cameraZoom == null) {
            cameraZoom = FindAnyObjectByType<CameraZoom>();
        }
    }

    void Update() {
        if (!playerNearby) return;

        if (dialogueActive && Input.GetKeyDown(KeyCode.X)) {
            if (isTyping) {
                StopAllCoroutines();
                dialogueText.text = lines[currentLine];
                isTyping = false;

                if (continuePrompt != null) {
                    continuePrompt.SetActive(true);
                }
            } else {
                NextLine();
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("DIALOGUE TRIGGER HIT: " + other.name);

        if (!other.CompareTag("Player")) return;

        Debug.Log("PLAYER ENTERED DIALOGUE TRIGGER");

        playerNearby = true;

        // CAMERA DISABLED FOR DEBUG
        // if (cameraFocus != null) {
        //     cameraFocus.FocusOn(transform);
        // }

        // if (cameraZoom != null) {
        //     cameraZoom.ZoomIn();
        // }

        StartDialogue();
    }

    void OnTriggerExit(Collider other) {
        Debug.Log("DIALOGUE TRIGGER EXIT: " + other.name);

        if (!other.CompareTag("Player")) return;

        playerNearby = false;

        // CAMERA DISABLED FOR DEBUG
        // if (cameraFocus != null) {
        //     cameraFocus.StopFocus();
        // }

        // if (cameraZoom != null) {
        //     cameraZoom.ZoomOut();
        // }

        EndDialogue();
    }

    void StartDialogue() {
        if (dialogueActive) return;

        dialogueActive = true;
        currentLine = 0;

        dialogueUI.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PopIn());

        if (audioSource != null && popSound != null) {
            audioSource.PlayOneShot(popSound);
        }

        ShowLine();
    }

    void EndDialogue() {
        dialogueActive = false;
        StopAllCoroutines();
        StartCoroutine(PopOut());

        if (continuePrompt != null) {
            continuePrompt.SetActive(false);
        }
    }

    void ShowLine() {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        isTyping = true;
        dialogueText.text = "";

        if (continuePrompt != null) {
            continuePrompt.SetActive(false);
        }

        foreach (char c in lines[currentLine]) {
            dialogueText.text += c;

            if (audioSource != null && speechSounds.Length > 0 && c != ' ') {
                if (Random.value > 0.6f) {
                    audioSource.pitch = Random.Range(minPitch, maxPitch);
                    audioSource.PlayOneShot(speechSounds[Random.Range(0, speechSounds.Length)]);
                }
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (continuePrompt != null) {
            continuePrompt.SetActive(true);
        }
    }

    void NextLine() {
        currentLine++;

        if (currentLine >= lines.Length) {

            if (!preventAutoClose) {
                EndDialogue();
            }

            if (onDialogueFinished != null) {
                onDialogueFinished.Invoke();
            }

        } else {
            ShowLine();
        }
    }

    IEnumerator PopIn() {
        float t = 0;
        Vector3 start = Vector3.zero;
        Vector3 overshoot = originalScale * popScale;

        while (t < 1) {
            t += Time.deltaTime * popSpeed;
            dialogueUI.transform.localScale = Vector3.Lerp(start, overshoot, t);
            yield return null;
        }

        t = 0;
        while (t < 1) {
            t += Time.deltaTime * popSpeed;
            dialogueUI.transform.localScale = Vector3.Lerp(overshoot, originalScale, t);
            yield return null;
        }
    }

    IEnumerator PopOut() {
        float t = 0;
        Vector3 start = dialogueUI.transform.localScale;

        while (t < 1) {
            t += Time.deltaTime * popSpeed;
            dialogueUI.transform.localScale = Vector3.Lerp(start, Vector3.zero, t);
            yield return null;
        }

        dialogueUI.SetActive(false);
    }
}