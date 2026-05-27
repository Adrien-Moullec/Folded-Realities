using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour {

    public static GraphicsSettings Instance;
    // Global URP volume reference
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
    // Cached values used for reverting changes
    float cachedBrightness;
    float cachedSaturation;
    float cachedVolume;

    void Awake() {
        Instance = this;
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #region Volume & Colour Adjustments 

    void Start() {
        // Finds volume and colour adjustment settings
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
        // Reconnects volume references after scene load
        SetupVolume();

        brightnessSlider = GameObject.Find("Brightness")?.GetComponent<Slider>();
        saturationSlider = GameObject.Find("Saturation")?.GetComponent<Slider>();
        volumeSlider = GameObject.Find("Volume")?.GetComponent<Slider>();

        ApplySettings();
        UpdateSliders();
        SetupAudioSlider();
    }

    void SetupVolume() {
        // Finds active global volume in scene
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
            // Clears old listeners
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }
    #endregion

    #region Cached Settings
    public void CacheCurrentSettings() {
        // Stores current settings for revert button
        cachedBrightness = brightness;
        cachedSaturation = saturation;
        cachedVolume = volume;
    }

    public void RevertCachedSettings() {

        brightness = cachedBrightness;
        saturation = cachedSaturation;
        volume = cachedVolume;
        AudioListener.volume = volume;
        // Reapplies reverted settings
        ApplySettings();
        UpdateSliders();
    }
    #endregion

    #region Save Settings & Update
    public void SaveSettings() {

        GameplaySystem.SetSettingsFloat(SettingsFloatPref.Brightness, brightness);
        GameplaySystem.SetSettingsFloat(SettingsFloatPref.Saturation, saturation);
        GameplaySystem.SetSettingsFloat(SettingsFloatPref.GameVolume, volume);
        // Writes settings to save file
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
#endregion