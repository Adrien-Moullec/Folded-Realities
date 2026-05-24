using UnityEngine;
using UnityEngine.UI;

public class BrightnessSettings : MonoBehaviour {
    public Slider slider;
    public Image preview1;
    public Image preview2;

    float tempValue = 1f;

    void Start() {
        slider.minValue = 0.5f;
        slider.maxValue = 1.5f;
        tempValue = GameplaySystem.GetSettingsFloat(SettingsFloatPref.Brightness, 1f);

        slider.value = tempValue;

        UpdatePreview();
    }

    public void OnSliderChanged(float value) {
        tempValue = value;
        UpdatePreview();
    }

    void UpdatePreview() {
        Color c = new Color(tempValue, tempValue, tempValue, 1f);

        preview1.color = c;
        preview2.color = c;
    }

    public void Accept() {
        GameplaySystem.SetSettingsFloat(SettingsFloatPref.Brightness, tempValue);
        GameplaySystem.SaveSettings();
    }

    public void Revert() {
        tempValue = GameplaySystem.GetSettingsFloat(SettingsFloatPref.Brightness, 1);
        slider.value = tempValue;
        UpdatePreview();
    }
}