using UnityEngine;

public class GameProgress : MonoBehaviour {

    public static GameProgress Instance;

    public int progress;

    void Awake() {

        if (
            Instance != null
            &&
            Instance != this
        ) {

            Destroy(gameObject);

            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        progress =
            PlayerPrefs.GetInt(
                "Progress",
                0
            );

        Debug.Log(
            "Loaded Progress = "
            + progress
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