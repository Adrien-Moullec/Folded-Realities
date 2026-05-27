using UnityEngine;

using System.Collections;

public class PianoKeyManager : MonoBehaviour {

    public PlatformGroup[] keyGroups;

    [Header("Correct Sequence")]
    public int[] correctSequence;
    // Correct key sequence for puzzle completion
    [Header("Memory Reveal Platforms")]
    public PlatformGroup[] memoryRevealPlatforms;

    PianoKey[] keys;

    int currentIndex = 0;
    // Tracks puzzle completion state
    bool puzzleComplete = false;
    // Prevents intro replaying multiple times
    bool introStarted = false;

    bool replayingMemory = false;

    [Header("Wrong Sound")]
    public AudioSource audioSource;

    public AudioClip wrongSound;
    // Melody played after puzzle completion
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
    }
    #region Start Puzzle & Intro Sequence

    public void StartMemoryPuzzle() {

        if (
            introStarted
        ) {
            return;
        }
        // Starts memory reveal sequence
        StartCoroutine(
            IntroSequence()
        );
    }

    IEnumerator IntroSequence() {

        introStarted = true;

        replayingMemory = true;

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
        // Holds completed reveal briefly
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
        ShowKeyPlatforms(
            keyID
        );
        // Checks if correct key was pressed
        if (
            keyID
            ==
            correctSequence[currentIndex]
        ) {

            currentIndex++;

            if (
                currentIndex
                >= correctSequence.Length
            ) {
                CompletePuzzle();
            }

            return true;
        }
        // Starts incorrect sequence reset
        StartCoroutine(
            WrongSequence()
        );

        return false;
    }
    #endregion

    #region Wrong Sequence & Reset 
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
        // Resets player progress
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
        // Replays memory reveal sequence
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
        // Resets all piano key states
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
        // Shows platforms linked to selected key
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
    #endregion

    #region Completed Puzzle
    void CompletePuzzle() {

        puzzleComplete = true;

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
        // Prevents playback without audio source
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
#endregion