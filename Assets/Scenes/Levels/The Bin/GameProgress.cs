using UnityEngine;

public class GameProgress : MonoBehaviour {

    public static GameProgress Instance;

    public int progress;

    void Awake() {

        Instance = this;

        progress =
            PlayerPrefs.GetInt(
                "Progress",
                0
            );
    }

    public void SetProgress(
        int value
    ) {

        progress = value;

        PlayerPrefs.SetInt(
            "Progress",
            progress
        );

        PlayerPrefs.Save();

        Debug.Log(
            "PROGRESS = "
            + progress
        );
    }
}