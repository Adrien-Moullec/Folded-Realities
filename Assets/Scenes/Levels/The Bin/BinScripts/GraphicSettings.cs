using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour {

    public static GraphicsSettings Instance;

    [Header("Graphics")]
    public Volume globalVolume;
    public Slider brightnessSlider;
    public Slider saturationSlider;
    ColorAdjustments colorAdjustments;
    float brightness;
    float saturation;

    [Header("Audio")]
    public Slider volumeSlider;
    float volume;
    float cachedBrightness;
    float cachedSaturation;
    float cachedVolume;

    void Awake() {
        Instance = this;
        SceneManager.sceneLoaded +=
            OnSceneLoaded;
    }



    void Start() {

        SetupVolume();
        brightness = GameplaySystem.GetSettingsFloat(SettingsFloatPref.Brightness, 0);
        saturation = GameplaySystem.GetSettingsFloat(SettingsFloatPref.Saturation, 0);
        volume = GameplaySystem.GetSettingsFloat(SettingsFloatPref.GameVolume, 0);
        ApplySettings();
        UpdateSliders();
        SetupAudioSlider();
    }

    void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    ) {

        SetupVolume();

        brightnessSlider = GameObject.Find("Brightness")?.GetComponent<Slider>();
        saturationSlider = GameObject.Find("Saturation")?.GetComponent<Slider>();
        volumeSlider = GameObject.Find("Volume")?.GetComponent<Slider>();

        ApplySettings();
        UpdateSliders();
        SetupAudioSlider();
    }

    void SetupVolume() {
        globalVolume = FindFirstObjectByType<Volume>();

        if (globalVolume == null)
            return;

        if (globalVolume.profile == null)
            return;


        bool found = globalVolume.profile.TryGet(out colorAdjustments);
    }

    void SetupAudioSlider() {
        AudioListener.volume = volume;

        if (volumeSlider != null) {
            volumeSlider.value = volume;
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void CacheCurrentSettings() {

        cachedBrightness = brightness;
        cachedSaturation = saturation;
        cachedVolume = volume;
    }

    public void RevertCachedSettings() {

        brightness = cachedBrightness;
        saturation = cachedSaturation;
        volume = cachedVolume;
        AudioListener.volume = volume;

        ApplySettings();
        UpdateSliders();
    }

    public void SaveSettings() {

        GameplaySystem.SetSettingsFloat(SettingsFloatPref.Brightness, brightness);
        GameplaySystem.SetSettingsFloat(SettingsFloatPref.Saturation, saturation);
        GameplaySystem.SetSettingsFloat(SettingsFloatPref.GameVolume, volume);
        GameplaySystem.SaveSettings();
    }

    public void SetBrightness(float value) {
        brightness = value;
        ApplySettings();
    }

    public void SetSaturation(float value) {
        saturation = value;
        ApplySettings();
    }

    public void SetVolume(float value) {
        volume = value;
        AudioListener.volume = volume;
    }

    void ApplySettings() {
        if (colorAdjustments != null) {
            colorAdjustments.postExposure.value = brightness;
            colorAdjustments.saturation.value = saturation;
        }
    }

    void UpdateSliders() {
        if (brightnessSlider != null)
            brightnessSlider.value = brightness;

        if (saturationSlider != null)
            saturationSlider.value = saturation;

        if (volumeSlider != null)
            volumeSlider.value = volume;
    }
}