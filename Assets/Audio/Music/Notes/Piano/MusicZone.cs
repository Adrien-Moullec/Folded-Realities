using UnityEngine;
using System.Collections;

public class MusicZone : MonoBehaviour {

    [Header("Music")]
    public AudioSource musicSource;

    [Range(0f, 1f)]
    public float quietVolume = 0.15f;

    [Range(0f, 1f)]
    public float normalVolume = 1f;

    // Speed of music fade
    public float fadeSpeed = 2f;

    [Header("UI")]
    public GameObject heartsCanvas;

    Coroutine fadeRoutine;

    void OnTriggerEnter(Collider other) {

        // Lowers music volume when player enters
        if (!other.CompareTag("Player"))
            return;

        StartFade(quietVolume);

        if (heartsCanvas != null)
            heartsCanvas.SetActive(false);
    }

    void OnTriggerExit(Collider other) {

        // Restores music volume when player leaves
        if (!other.CompareTag("Player"))
            return;

        StartFade(normalVolume);

        if (heartsCanvas != null)
            heartsCanvas.SetActive(true);
    }

    void StartFade(float target) {

        // Stops previous fade before starting new one
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeVolume(target));
    }

    IEnumerator FadeVolume(float target) {

        // Smoothly fades audio volume
        while (Mathf.Abs(musicSource.volume - target) > 0.01f) {

            musicSource.volume = Mathf.Lerp(
                musicSource.volume,
                target,
                Time.deltaTime * fadeSpeed
            );

            yield return null;
        }

        musicSource.volume = target;
    }
}