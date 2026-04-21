using UnityEngine;

public class TrashStuckZone : MonoBehaviour {
    [SerializeField] GameObject resetPlatform;
    [SerializeField] GameObject stuckPrompt;

    void Start() {
        resetPlatform.SetActive(false);
        stuckPrompt.SetActive(false);
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Trash")) {
            return;
        }

        Debug.Log("Trash stuck — enabling reset");

        resetPlatform.SetActive(true);
        stuckPrompt.SetActive(true);
    }
}