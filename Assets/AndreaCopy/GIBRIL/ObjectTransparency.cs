using UnityEngine;

public class ObjectTransparency : MonoBehaviour {
    [SerializeField] private float transparentAlpha = 0.25f;
    [SerializeField] private float fadeSpeed = 6f;
    Renderer[] rends;
    Material[] mats;
    float[] originalAlphas;
    bool shouldBeTransparent;

    void Start() {
        rends = GetComponentsInChildren<Renderer>();
        mats = new Material[rends.Length];
        originalAlphas = new float[rends.Length];

        for (int i = 0; i < rends.Length; i++) {
            mats[i] = rends[i].material;
            Color c = mats[i].color;
            originalAlphas[i] = c.a;
        }
    }

    void Update() {
        for (int i = 0; i < mats.Length; i++) {
            Color c = mats[i].color;
            float targetAlpha = shouldBeTransparent ? transparentAlpha : originalAlphas[i];
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
            mats[i].SetFloat("_Alpha", c.a);
        }
    }

    public void SetTransparent(bool value) {
        shouldBeTransparent = value;
    }
}