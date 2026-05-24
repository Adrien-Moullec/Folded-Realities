using UnityEngine;

public class ProgressSetter : MonoBehaviour {

    public int setProgressTo;

    bool triggered;

    void OnTriggerEnter(
        Collider other
    ) {

        if (
            triggered
            ||
            !other.CompareTag(
                "Player"
            )
        ) {
            return;
        }

        triggered = true;

        int currentProgress = GameplaySystem.GetInt(PrefInt.Progress, 0);

        if (
            setProgressTo >
            currentProgress
        ) {
            GameplaySystem.GetInt(PrefInt.Progress, setProgressTo);
        }
    }
}