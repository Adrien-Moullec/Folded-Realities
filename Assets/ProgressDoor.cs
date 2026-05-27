using UnityEngine;

public class ProgressDoor : MonoBehaviour {

    public int minProgress;

    public int maxProgress = 999;

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

        bool unlocked =
            (
                progress >= minProgress
                &&
                progress <= maxProgress
            )
            ||
            progress >= 3;

        if (levelExit != null) {

            levelExit.locked =
                !unlocked;
        }

        if (lockVisual != null) {

            lockVisual.SetActive(
                !unlocked
            );
        }
    }
}