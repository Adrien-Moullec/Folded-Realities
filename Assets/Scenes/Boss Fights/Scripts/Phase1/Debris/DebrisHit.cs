using UnityEngine;
using System.Collections;

public class DebrisHit : MonoBehaviour {
    private Renderer rend;
    private Color originalColor;

    [Header("Flash Settings")]
    public Color flashColor = Color.red;   
    public float flashDuration = 0.2f;

    void Start() {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    public void Flash() {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine() {
        rend.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = originalColor;
    }
}