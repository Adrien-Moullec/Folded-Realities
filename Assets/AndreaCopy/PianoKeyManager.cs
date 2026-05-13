using UnityEngine;

public class PianoKeyManager : MonoBehaviour {

    public PlatformGroup[] keyGroups;

    public GameObject[] keys;

    int activatedKeys = 0;

    bool puzzleComplete = false;

    void Start() {

        activatedKeys = 0;

        puzzleComplete = false;

        for (
            int i = 0;
            i < keyGroups.Length;
            i++
        ) {
            keyGroups[i].Hide();
        }

        Debug.Log(
            "Puzzle Initialized"
        );
    }

    public void PressKey(
        int keyID
    ) {

        if (puzzleComplete) {
            return;
        }

        activatedKeys++;

        Debug.Log(
            "Activated Key: "
            + keyID
        );

        ShowKeyPlatforms(
            keyID
        );

        if (
            activatedKeys
            >= keys.Length
        ) {
            CompletePuzzle();
        }
    }

    void ShowKeyPlatforms(
        int keyID
    ) {

        for (
            int i = 0;
            i < keyGroups.Length;
            i++
        ) {

            if (
                keyGroups[i].keyID
                == keyID
            ) {
                keyGroups[i].Show();

                Debug.Log(
                    "Showing platforms for key: "
                    + keyID
                );
            }
        }
    }

    void CompletePuzzle() {

        puzzleComplete = true;

        Debug.Log(
            "ALL COLOURS ACTIVATED"
        );

        for (
            int i = 0;
            i < keyGroups.Length;
            i++
        ) {
            keyGroups[i].Show();
        }
    }
}