using UnityEngine;
using System.Collections;

public class PianoKeyManager : MonoBehaviour {

    public PlatformGroup[] keyGroups;

    [Header("Correct Sequence")]
    public int[] correctSequence;

    PianoKey[] keys;

    int currentIndex = 0;

    bool puzzleComplete = false;

    [Header("Wrong Sound")]
    public AudioSource audioSource;

    public AudioClip wrongSound;

    [Header("Victory Melody")]
    public AudioClip[] victoryMelody;

    public float melodyDelay = 0.4f;

    void Start() {

        keys =
    FindObjectsByType<PianoKey>(
        FindObjectsSortMode.None
    );

        currentIndex = 0;

        puzzleComplete = false;

        for (
            int i = 0;
            i < keyGroups.Length;
            i++
        ) {
            keyGroups[i].Hide();
        }

        ResetAllKeys();

        Debug.Log(
            "Memory Puzzle Initialized"
        );
    }

    public bool PressKey(
        int keyID
    ) {

        if (puzzleComplete) {
            return false;
        }

        Debug.Log(
            "Pressed Key: "
            + keyID
        );

        if (
            keyID
            ==
            correctSequence[currentIndex]
        ) {

            Debug.Log(
                "Correct Key"
            );

            ShowKeyPlatforms(
                keyID
            );

            currentIndex++;

            if (
                currentIndex
                >= correctSequence.Length
            ) {
                CompletePuzzle();
            }

            return true;
        }

        Debug.Log(
            "WRONG KEY"
        );

        StartCoroutine(
            WrongSequence()
        );

        return false;
    }

    IEnumerator WrongSequence() {

        if (
            audioSource != null
            && wrongSound != null
        ) {
            audioSource.PlayOneShot(
                wrongSound
            );
        }

        yield return new WaitForSeconds(
            0.2f
        );

        currentIndex = 0;

        for (
            int i = 0;
            i < keyGroups.Length;
            i++
        ) {
            keyGroups[i].Hide();
        }

        ResetAllKeys();
    }

    void ResetAllKeys() {

        for (
            int i = 0;
            i < keys.Length;
            i++
        ) {

            if (
                keys[i] != null
            ) {
                keys[i].ResetKey();
            }
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
            }
        }
    }

    void CompletePuzzle() {

        puzzleComplete = true;

        Debug.Log(
            "PUZZLE COMPLETE"
        );

        for (
            int i = 0;
            i < keyGroups.Length;
            i++
        ) {
            keyGroups[i].Show();
        }

        StartCoroutine(
            PlayVictoryMelody()
        );
    }

    IEnumerator PlayVictoryMelody() {

        if (
            audioSource == null
        ) {
            yield break;
        }

        for (
            int i = 0;
            i < victoryMelody.Length;
            i++
        ) {

            if (
                victoryMelody[i] != null
            ) {

                audioSource.PlayOneShot(
                    victoryMelody[i]
                );

                yield return new WaitForSeconds(
                    melodyDelay
                );
            }
        }
    }
}