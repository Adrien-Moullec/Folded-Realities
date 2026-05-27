using UnityEngine;

public class StageLock : MonoBehaviour {

    public string requiredKey;

    public GameObject lockVisual;
    public GameObject lockedUI;

    bool isUnlocked = false;

    LevelExit levelExit;

    void Start() {

        levelExit = GetComponent<LevelExit>();

        CheckState();
    }

    void OnTriggerEnter(Collider other) {

        // Only checks player collisions
        if (!other.CompareTag("Player"))
            return;

        CheckState();

        // Shows locked UI if stage is unavailable
        if (!isUnlocked && lockedUI != null)
            lockedUI.SetActive(true);
    }

    void CheckState() {

        // Checks whether player owns required key
        if (GameplaySystem.GetInt(PrefInt.OwnsKey, 0) == 1)
            Unlock();
        else
            Lock();
    }

    void Unlock() {

        isUnlocked = true;

        // Hides lock visuals
        if (lockVisual != null)
            lockVisual.SetActive(false);

        // Enables level transition
        if (levelExit != null)
            levelExit.enabled = true;
    }

    void Lock() {

        isUnlocked = false;

        // Shows lock visuals
        if (lockVisual != null)
            lockVisual.SetActive(true);

        // Disables level transition
        if (levelExit != null)
            levelExit.enabled = false;
    }
}