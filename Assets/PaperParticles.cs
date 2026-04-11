using System;

using UnityEngine;
using UnityEngine.VFX;

public class PaperParticles : MonoBehaviour, IDelta {
    VisualEffect visualEffect;
    void Awake() {
        TryGetComponent(out visualEffect);
        EndDelta();
    }
    public void StartDelta() {
        visualEffect.Play();
        visualEffect.enabled = true;
    }
    public void UpdateDelta(float delta) {
        gameObject.transform.localScale = new Vector3(delta, delta, delta);
    }
    public void EndDelta() {
        UpdateDelta(0);
        visualEffect.Stop();
        visualEffect.enabled = false;
    }
}
