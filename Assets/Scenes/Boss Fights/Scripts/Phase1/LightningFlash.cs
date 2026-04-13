using UnityEngine;

public class LightningFlash : MonoBehaviour {
    public float minDelay = 2f;
    public float maxDelay = 6f;
    public float flashIntensity = 2f;
    public float flashDuration = 0.1f;

    private Light lightningLight;

    void Start() {
        lightningLight = GetComponent<Light>();
        StartCoroutine(FlashRoutine());
    }

    System.Collections.IEnumerator FlashRoutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            lightningLight.intensity = flashIntensity;
            yield return new WaitForSeconds(flashDuration);
            lightningLight.intensity = 0;
        }
    }
}