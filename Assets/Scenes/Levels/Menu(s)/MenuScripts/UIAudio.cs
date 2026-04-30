using UnityEngine;

public class UIAudio : MonoBehaviour {
    public static UIAudio Instance;

    public AudioSource source;

    public AudioClip hoverSound;
    public AudioClip clickSound;

    [Header("Timing")]
    public float clickDuration = 0.15f;

    [Header("Pitch")]
    public float hoverPitch = 1f;
    public float clickPitch = 1f;

    void Awake() {
        Instance = this;
    }

    public void PlayHover() {
        source.pitch = hoverPitch;
        source.PlayOneShot(hoverSound);
    }

    public void PlayClick() {
        StopAllCoroutines();
        StartCoroutine(PlayClickTrimmed());
    }

    System.Collections.IEnumerator PlayClickTrimmed() {
        source.pitch = clickPitch;
        source.clip = clickSound;
        source.time = 0f;
        source.Play();

        yield return new WaitForSeconds(clickDuration);

        source.Stop();
    }
}