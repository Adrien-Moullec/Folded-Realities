using UnityEngine;

public class ProgressDoor : MonoBehaviour {

    [Header("Progress Requirements")]
    public int minProgress;

    public int maxProgress = 999;

    [Header("Visuals")]
    public GameObject lockVisual;

    LevelExit levelExit;

    void Start() {

        levelExit =
            GetComponent<LevelExit>();

        UpdateDoor();
    }

    void Update() {

        UpdateDoor();
    }

    void UpdateDoor() {

        if (GameProgress.Instance == null)
            return;

        int progress =
            GameProgress.Instance.progress;

        bool unlocked = false;

        // Normal unlock range
        if (
            progress >= minProgress
            &&
            progress <= maxProgress
        ) {

            unlocked = true;
        }

        // Once player reaches progress 3
        // all lobby doors unlock permanently
        if (progress >= 3) {

            unlocked = true;
        }

        // Apply lock state
        if (levelExit != null) {

            levelExit.locked =
                !unlocked;
        }

        // Toggle lock visual
        if (lockVisual != null) {

            lockVisual.SetActive(
                !unlocked
            );
        }

        Debug.Log(
            gameObject.name +
            " | Progress = " +
            progress +
            " | Unlocked = " +
            unlocked
        );
    }
}