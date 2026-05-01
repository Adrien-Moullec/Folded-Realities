using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GraphicsSettings : MonoBehaviour {

    public static GraphicsSettings Instance;

    public Volume globalVolume;

    ColorAdjustments colorAdjustments;

    float brightness;
    float saturation;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start() {
        SetupVolume();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        SetupVolume();
        ApplySettings();
    }

    void SetupVolume() {

        if (globalVolume == null) {
            globalVolume = FindObjectOfType<Volume>();
        }

        if (globalVolume != null && globalVolume.profile != null) {
            globalVolume.profile.TryGet(out colorAdjustments);
        }

        brightness = PlayerPrefs.GetFloat("Brightness", 0f);
        saturation = PlayerPrefs.GetFloat("Saturation", 0f);
    }

    public void SetBrightness(float value) {
        brightness = value;
        PlayerPrefs.SetFloat("Brightness", brightness);
        ApplySettings();
    }

    public void SetSaturation(float value) {
        saturation = value;
        PlayerPrefs.SetFloat("Saturation", saturation);
        ApplySettings();
    }

    void ApplySettings() {
        if (colorAdjustments != null) {
            colorAdjustments.postExposure.value = brightness;
            colorAdjustments.saturation.value = saturation;
        }
    }
}