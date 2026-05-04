using UnityEngine;

public class PianoKeyManager : MonoBehaviour {

    public int[] correctSequence;
    public PlatformGroup[] keyGroups;
    public GameObject[] keys;

    private int currentIndex = 0;
    private bool puzzleComplete = false;

    void Start() {

        currentIndex = 0;
        puzzleComplete = false;

        for (int i = 0; i < keyGroups.Length; i++) {
            keyGroups[i].Hide();
        }

        Debug.Log("Puzzle Initialized");
    }

    public void PressKey(int keyID) {

        if (puzzleComplete) {
            return;
        }

        if (currentIndex >= correctSequence.Length) {
            return;
        }

        Debug.Log("Pressed: " + keyID + " | Expected: " + correctSequence[currentIndex]);

        if (keyID == correctSequence[currentIndex]) {

            Debug.Log("Correct Step: " + currentIndex);

            ShowKeyPlatforms(keyID);

            currentIndex++;

            if (currentIndex >= correctSequence.Length) {
                CompletePuzzle();
            }

        } else {

            Debug.Log("WRONG KEY - RESET");
            ResetPuzzle();
        }
    }

    void ShowKeyPlatforms(int keyID) {

        for (int i = 0; i < keyGroups.Length; i++) {

            if (keyGroups[i].keyID == keyID) {
                keyGroups[i].Show();
                Debug.Log("Showing platforms for key: " + keyID);
            }
        }
    }

    void CompletePuzzle() {

        puzzleComplete = true;

        Debug.Log("SEQUENCE COMPLETE");

        for (int i = 0; i < keyGroups.Length; i++) {
            keyGroups[i].Show();
        }
    }

    void ResetPuzzle() {

        if (puzzleComplete) {
            return;
        }

        currentIndex = 0;

        for (int i = 0; i < keyGroups.Length; i++) {
            keyGroups[i].Hide();
        }
    }
}