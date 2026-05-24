/*using UnityEngine;

public class GameProgress : MonoBehaviour {

    public static GameProgress Instance;

    public int progress;

    void Awake() {

        Instance = this;

        progress =
            PlayersPrefs.GetInt(
                "Progress",
                0
            );
    }

    public void SetProgress(
        int value
    ) {

        progress = value;

        PlayersPrefs.SetInt(
            "Progress",
            progress
        );

        PlayersPrefs.Save();

        Debug.Log(
            "PROGRESS = "
            + progress
        );
    }
}*/