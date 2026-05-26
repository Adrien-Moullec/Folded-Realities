using UnityEngine;

public class StageLock : MonoBehaviour {

    public string requiredKey;

    public GameObject lockVisual;

    public GameObject lockedUI;

    bool isUnlocked = false;

    LevelExit levelExit;

    void Start() {

        levelExit =
            GetComponent<LevelExit>();

        CheckState();
    }

    void OnTriggerEnter(
        Collider other
    ) {

        if (
            !other.CompareTag(
                "Player"
            )
        ) {
            return;
        }

        CheckState();

        if (
            !isUnlocked
            &&
            lockedUI != null
        ) {

            lockedUI.SetActive(
                true
            );
        }
    }

    void CheckState() {

        if (GameplaySystem.GetInt(PrefInt.OwnsKey, 0) == 1) {
            Unlock();
        } else {
            Lock();
        }
    }

    void Unlock() {

        if (lockVisual != null) {

            lockVisual.SetActive(
                false
            );
        }

        if (levelExit != null) {

            levelExit.enabled =
                true;
        }
    }

    void Lock() {

        if (lockVisual != null) {

            lockVisual.SetActive(
                true
            );
        }

        if (levelExit != null) {

            levelExit.enabled =
                false;
        }
    }
}