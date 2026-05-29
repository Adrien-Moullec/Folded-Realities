using System;

using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Visual effect for paper particles to control how it looks over time
/// </summary>
public class PaperParticles : MonoBehaviour, IDelta {
    VisualEffect visualEffect;
    void Awake() {
        TryGetComponent(out visualEffect);
        EndDelta();
    }
    public void StartDelta() {
        visualEffect.enabled = true;
        visualEffect.Play();
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
