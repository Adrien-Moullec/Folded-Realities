using System.Collections;

using UnityEngine;

using TMPro;
using UnityEngine.Events;

public class NPCDialogue : MonoBehaviour {



    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public GameObject continuePrompt;
    // Dialogue lines displayed in sequence
    [TextArea(2, 5)]
    public string[] lines;
    public float typingSpeed = 0.03f;

    public float popSpeed = 6f;
    public float popScale = 1.2f;

    public AudioSource audioSource;
    public AudioClip popSound;
    public AudioClip[] speechSounds;
    // Random pitch variation for speech sounds
    public float minPitch = 0.8f;
    public float maxPitch = 1.0f;

    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueFinished;
    public bool preventAutoClose = false;

    public bool autoAdvance = false;
    public float autoAdvanceDelay = 1.5f;

    public bool dontDestroyAfterDialogue = false;

    private int currentLine = 0;
    private bool playerNearby = false;
    private bool isTyping = false;
    private bool dialogueActive = false;

    private Vector3 originalScale;

    void Start() {
        if (dialogueUI != null) {
            dialogueUI.SetActive(false);
        }
        // Stores original scale for pop animation
        originalScale = dialogueUI.transform.localScale;
        dialogueUI.transform.localScale = Vector3.zero;

        if (continuePrompt != null) {
            continuePrompt.SetActive(false);
        }
    }

    void Update() {
        if (!playerNearby || autoAdvance) return;
        // Prevents manual input during auto advance
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
        // Starts dialogue when player enters trigger
        if (!other.CompareTag("Player")) return;

        playerNearby = true;

        GetComponent<Collider>().enabled = false;

        StartDialogue();
    }
    // Tracks when player leaves dialogue area
    void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player")) return;

        playerNearby = false;
    }

    void StartDialogue() {
        if (dialogueActive || dialogueUI == null) return;

        dialogueActive = true;
        currentLine = 0;

        dialogueUI?.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PopIn());

        if (audioSource != null && popSound != null) {
            audioSource.PlayOneShot(popSound);
        }

        ShowLine();
    }

    void EndDialogue() {
        // Ends active dialogue session
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
        // Hides continue prompt while typing
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

        if (autoAdvance) {
            yield return new WaitForSeconds(autoAdvanceDelay);
            NextLine();
        } else {
            if (continuePrompt != null) {
                continuePrompt.SetActive(true);
            }
        }
    }

    void NextLine() {
        currentLine++;

        if (currentLine >= lines.Length) {

            if (!preventAutoClose) {
                EndDialogue();
            }
            // Invokes dialogue finished event
            if (onDialogueFinished != null) {
                onDialogueFinished.Invoke();
            }

            if (!dontDestroyAfterDialogue) {
                StartCoroutine(DestroyAfter());
            }

        } else {
            ShowLine();
        }
    }

    IEnumerator DestroyAfter() {
        yield return new WaitForSeconds(0.3f);

        if (dialogueUI != null) {
            dialogueUI.SetActive(false);
        }

        Destroy(gameObject);
    }

    IEnumerator PopIn() {
        // Pop-in UI animation
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
        // Hides UI after animation completes
        dialogueUI.SetActive(false);
    }
}