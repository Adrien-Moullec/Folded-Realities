using UnityEngine;
using System.Collections;

public class PianoKeyManager : MonoBehaviour {

    public PlatformGroup[] keyGroups;

    [Header("Correct Sequence")]
    public int[] correctSequence;

    [Header("Memory Reveal Platforms")]
    public PlatformGroup[] memoryRevealPlatforms;

    PianoKey[] keys;

    int currentIndex = 0;

    bool puzzleComplete = false;

    bool introStarted = false;

    bool replayingMemory = false;

    [Header("Wrong Sound")]
    public AudioSource audioSource;

    public AudioClip wrongSound;

    [Header("Victory Melody")]
    public AudioClip[] victoryMelody;

    public float melodyDelay = 0.4f;

    [Header("Memory Timing")]
    public float flashOffTime = 0.12f;

    public float platformRevealDelay = 0.8f;

    public float introVisibleTime = 1.5f;

    [Header("Trigger")]
    public PianoPuzzleTrigger puzzleTrigger;

    void Start() {

        keys =
    FindObjectsByType<PianoKey>(
        FindObjectsSortMode.None
    );

        ResetAllKeys();

        Debug.Log(
            "Memory Puzzle Initialized"
        );
    }

    public void StartMemoryPuzzle() {

        if (
            introStarted
        ) {
            return;
        }

        StartCoroutine(
            IntroSequence()
        );
    }

    IEnumerator IntroSequence() {

        introStarted = true;

        replayingMemory = true;

        Debug.Log(
            "START MEMORY INTRO"
        );

        for (
            int i = 0;
            i < memoryRevealPlatforms.Length;
            i++
        ) {

            if (
                memoryRevealPlatforms[i] != null
            ) {

                memoryRevealPlatforms[i].Hide();

                yield return new WaitForSeconds(
                    flashOffTime
                );

                memoryRevealPlatforms[i].Show();

                yield return new WaitForSeconds(
                    platformRevealDelay
                );
            }
        }

        yield return new WaitForSeconds(
            introVisibleTime
        );

        for (
            int i = 0;
            i < keyGroups.Length;
            i++
        ) {

            if (
                keyGroups[i] != null
            ) {
                keyGroups[i].Hide();
            }
        }

        replayingMemory = false;

        Debug.Log(
            "MEMORY PUZZLE ACTIVE"
        );
    }

    public bool PressKey(
        int keyID
    ) {

        if (
            replayingMemory
        ) {
            return false;
        }

        if (
            puzzleComplete
        ) {

            ShowKeyPlatforms(
                keyID
            );

            return true;
        }

        if (
            !introStarted
        ) {
            return false;
        }

        Debug.Log(
            "Pressed Key: "
            + keyID
        );

        ShowKeyPlatforms(
            keyID
        );

        if (
            keyID
            ==
            correctSequence[currentIndex]
        ) {

            Debug.Log(
                "Correct Key"
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
            0.6f
        );

        currentIndex = 0;

        for (
            int i = 0;
            i < keyGroups.Length;
            i++
        ) {

            if (
                keyGroups[i] != null
            ) {
                keyGroups[i].Hide();
            }
        }

        ResetAllKeys();

        yield return new WaitForSeconds(
            0.4f
        );

        StartCoroutine(
            ReplayMemorySequence()
        );
    }

    IEnumerator ReplayMemorySequence() {

        replayingMemory = true;

        Debug.Log(
            "REPLAY MEMORY SEQUENCE"
        );

        for (
            int i = 0;
            i < memoryRevealPlatforms.Length;
            i++
        ) {

            if (
                memoryRevealPlatforms[i] != null
            ) {

                memoryRevealPlatforms[i].Hide();

                yield return new WaitForSeconds(
                    flashOffTime
                );

                memoryRevealPlatforms[i].Show();

                yield return new WaitForSeconds(
                    platformRevealDelay
                );
            }
        }

        yield return new WaitForSeconds(
            introVisibleTime
        );

        for (
            int i = 0;
            i < keyGroups.Length;
            i++
        ) {

            if (
                keyGroups[i] != null
            ) {
                keyGroups[i].Hide();
            }
        }

        replayingMemory = false;
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

            if (
                keyGroups[i] != null
            ) {
                keyGroups[i].Show();
            }
        }

        if (
            puzzleTrigger != null
        ) {
            puzzleTrigger.DestroyTrigger();
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