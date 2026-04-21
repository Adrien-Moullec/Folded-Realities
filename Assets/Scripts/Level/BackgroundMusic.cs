using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {
    public AudioSource audioSource;
    public float targetVolume = 0.2f;
    public float fadeDuration = 3f;

    void Start() {
        audioSource.volume = 0f;
        audioSource.loop = true;
        audioSource.Play();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn() {
        float t = 0f;

        while (t < fadeDuration) {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}