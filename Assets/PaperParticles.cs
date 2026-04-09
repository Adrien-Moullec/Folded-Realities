using System;

using UnityEngine;
using UnityEngine.VFX;

public class PaperParticles : MonoBehaviour, IDelta {

    [SerializeField] float speed = 2;
    VisualEffect visualEffect;
    float time = 0;
    float scale = 0;
    void Awake() {
        TryGetComponent(out visualEffect);
        SetDelta(1);
    }
    void Update() {
        time += Time.deltaTime * speed;
        scale = (Mathf.Sin(time) * 0.5f) + 0.5f;
        SetDelta(scale);
    }
    public void Start() {
        visualEffect.Play();
    }
    public void SetDelta(float delta) {
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }
    public void End() {
        visualEffect.Stop();
    }
}
