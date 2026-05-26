using UnityEngine;

public class UnlockLevel : MonoBehaviour {

    public string unlockKey;

    bool triggered = false;

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

        GameplaySystem.SetInt(PrefInt.UnlockKey, 1);
        GameplaySystem.SaveSettings();
    }
}