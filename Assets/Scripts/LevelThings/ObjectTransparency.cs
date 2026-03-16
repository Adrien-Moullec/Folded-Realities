using UnityEngine;

public class ObjectTransparency : MonoBehaviour {
    [SerializeField] private float transparentAlpha = 0.25f;
    [SerializeField] private float fadeSpeed = 6f;

    private Renderer rend;
    private Material mat;
    private float originalAlpha;

    private bool shouldBeTransparent;

    void Start() {
        rend = GetComponent<Renderer>();
        mat = rend.material;

        Color c = mat.color;
        originalAlpha = c.a;
    }

    void Update() {
        Color c = mat.color;

        float targetAlpha = shouldBeTransparent ? transparentAlpha : originalAlpha;

        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);

        mat.color = c;
    }

    public void SetTransparent(bool value) {
        shouldBeTransparent = value;
    }
}