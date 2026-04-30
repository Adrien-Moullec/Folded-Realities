using UnityEngine;
using UnityEngine.UI;

public class BrightnessSettings : MonoBehaviour {
    public Image overlay;
    public Slider slider;

    float tempValue;

    void OnEnable() {
        float saved = PlayerPrefs.GetFloat("Brightness", 1f);
        slider.value = saved;
        tempValue = saved;
        ApplyVisual(saved);
    }

    public void OnSliderChanged(float value) {
        tempValue = value;
        ApplyVisual(value);
    }

    void ApplyVisual(float value) {
        Color c = overlay.color;
        c.a = 1 - value;
        overlay.color = c;
    }

    public void Accept() {
        PlayerPrefs.SetFloat("Brightness", tempValue);
        PlayerPrefs.Save();
    }

    public void Revert() {
        float saved = PlayerPrefs.GetFloat("Brightness", 1f);
        slider.value = saved;
        ApplyVisual(saved);
    }
}