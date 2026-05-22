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

        brightness =
            PlayerPrefs.GetFloat(
                "Brightness",
                0f
            );

        saturation =
            PlayerPrefs.GetFloat(
                "Saturation",
                0f
            );

        volume =
            PlayerPrefs.GetFloat(
                "GameVolume",
                0.5f
            );

        ApplySettings();

        UpdateSliders();

        SetupAudioSlider();
    }

    void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    ) {
        Debug.Log(
      "SCENE LOADED: "
      + scene.name
  );


        SetupVolume();

        brightnessSlider =
            GameObject.Find(
                "Brightness"
            )?.GetComponent<Slider>();

        saturationSlider =
            GameObject.Find(
                "Saturation"
            )?.GetComponent<Slider>();

        volumeSlider =
            GameObject.Find(
                "Volume"
            )?.GetComponent<Slider>();

        ApplySettings();

        UpdateSliders();

        SetupAudioSlider();
    }

    void SetupVolume() {

        Debug.Log(
            "SETUP VOLUME RUNNING"
        );

        globalVolume =
            FindFirstObjectByType<Volume>();

        if (
            globalVolume == null
        ) {

            Debug.LogError(
                "NO GLOBAL VOLUME FOUND"
            );

            return;
        }

        Debug.Log(
            "FOUND VOLUME: "
            + globalVolume.name
        );

        if (
            globalVolume.profile == null
        ) {

            Debug.LogError(
                "NO PROFILE FOUND"
            );

            return;
        }

        bool found =
            globalVolume.profile.TryGet(
                out colorAdjustments
            );

        Debug.Log(
            "COLOR FOUND: "
            + found
        );
    }

    void SetupAudioSlider() {

        AudioListener.volume =
            volume;

        if (
            volumeSlider != null
        ) {

            volumeSlider.value =
                volume;

            volumeSlider.onValueChanged
                .RemoveAllListeners();

            volumeSlider.onValueChanged
                .AddListener(
                    SetVolume
                );
        }
    }

    public void CacheCurrentSettings() {

        cachedBrightness =
            brightness;

        cachedSaturation =
            saturation;

        cachedVolume =
            volume;
    }

    public void RevertCachedSettings() {

        brightness =
            cachedBrightness;

        saturation =
            cachedSaturation;

        volume =
            cachedVolume;

        AudioListener.volume =
            volume;

        ApplySettings();

        UpdateSliders();
    }

    public void SaveSettings() {

        PlayerPrefs.SetFloat(
            "Brightness",
            brightness
        );

        PlayerPrefs.SetFloat(
            "Saturation",
            saturation
        );

        PlayerPrefs.SetFloat(
            "GameVolume",
            volume
        );

        PlayerPrefs.Save();
    }

    public void SetBrightness(
        float value
    ) {

        brightness =
            value;

        ApplySettings();
    }

    public void SetSaturation(
        float value
    ) {

        saturation =
            value;

        ApplySettings();
    }

    public void SetVolume(
        float value
    ) {

        volume =
            value;

        AudioListener.volume =
            volume;
    }

    void ApplySettings() {

        if (
            colorAdjustments != null
        ) {

            colorAdjustments
                .postExposure
                .value =
                    brightness;

            colorAdjustments
                .saturation
                .value =
                    saturation;
        }
    }

    void UpdateSliders() {

        if (
            brightnessSlider != null
        ) {

            brightnessSlider.value =
                brightness;
        }

        if (
            saturationSlider != null
        ) {

            saturationSlider.value =
                saturation;
        }

        if (
            volumeSlider != null
        ) {

            volumeSlider.value =
                volume;
        }
    }
}