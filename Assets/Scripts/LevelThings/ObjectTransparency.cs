using UnityEngine;

public class ObjectTransparency : MonoBehaviour {
    public Transform player;
    public float radius = 5f;
    public float transparentAlpha = 0.25f;
    public float fadeSpeed = 6f;

    Renderer rend;
    Material mat;
    float originalAlpha;

    void Start() {
        rend = GetComponent<Renderer>();
        mat = rend.material;

        // Force material to transparent
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.renderQueue = 3000;

        Color c = mat.color;
        originalAlpha = c.a;
    }

    void Update() {
        if (!player) {
            return;
        }

        float dist = Vector3.Distance(player.position, transform.position);

        Color c = mat.color;

        float targetAlpha = dist < radius ? transparentAlpha : originalAlpha;

        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);

        mat.color = c;
    }
}