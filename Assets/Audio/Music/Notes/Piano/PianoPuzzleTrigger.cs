using UnityEngine;

public class PianoPuzzleTrigger : MonoBehaviour {

    public PianoKeyManager puzzleManager;

    bool triggered = false;

    void OnTriggerEnter(
        Collider other
    ) {

        if (
            triggered
        ) {
            return;
        }

        if (
            !other.CompareTag(
                "Player"
            )
        ) {
            return;
        }

        triggered = true;

        puzzleManager.StartMemoryPuzzle();
    }

    public void DestroyTrigger() {

        Destroy(
            gameObject
        );
    }
}