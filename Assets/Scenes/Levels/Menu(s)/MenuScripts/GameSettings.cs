using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour {
    public static GameSettings Instance;

    public Image brightnessOverlay;
    public Image saturationOverlay;

    void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        ApplySettings();
    }

    public void ApplySettings() {
        float brightness = PlayerPrefs.GetFloat("Brightness", 1f);
        float saturation = PlayerPrefs.GetFloat("Saturation", 1f);

        if (brightnessOverlay != null) {
            Color b = brightnessOverlay.color;
            b.a = 1 - brightness;
            brightnessOverlay.color = b;
        }

        if (saturationOverlay != null) {
            Color s = saturationOverlay.color;
            s.a = 1 - saturation;
            saturationOverlay.color = s;
        }
    }
}