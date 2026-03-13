using UnityEngine;
using TMPro;

public class SpeechBubbleTrigger : MonoBehaviour {
    public GameObject speechBubble;
    public TMP_Text speechText;

    [TextArea(3, 6)]
    public string message;

    private void Start() {
        speechBubble.SetActive(false);
        speechText.text = message;
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player"))
            return;

        speechBubble.SetActive(true);
    }

    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player"))
            return;

        speechBubble.SetActive(false);
    }
}