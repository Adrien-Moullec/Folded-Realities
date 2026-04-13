using UnityEngine;
using System.Collections;

public class PlatformDissolve : MonoBehaviour {
    public float disableTime = 5f;

    private Collider col;
    private Renderer rend;

    void Start() {
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
    }

    public void HitPlatform() {
        StopAllCoroutines();
        StartCoroutine(DissolveRoutine());
    }

    IEnumerator DissolveRoutine() {
        col.enabled = false;
        rend.enabled = false;

        yield return new WaitForSeconds(disableTime);

        col.enabled = true;
        rend.enabled = true;
    }
}