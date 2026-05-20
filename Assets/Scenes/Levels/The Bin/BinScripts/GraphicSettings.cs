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
    }

    void OnEnable() {

        SceneManager.sceneLoaded +=
            OnSceneLoaded;
    }

    void OnDisable() {

        SceneManager.sceneLoaded -=
            OnSceneLoaded;
    }

    void Start() {

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

        SetupVolume();

        FindSliders();

        ApplySettings();

        UpdateSliders();

        SetupAudioSlider();
    }

    void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    ) {

        SetupVolume();

        FindSliders();

        ApplySettings();

        UpdateSliders();

        SetupAudioSlider();
    }

    void FindSliders() {

        Slider[] sliders =
            FindObjectsByType<Slider>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (
            Slider s in sliders
        ) {

            if (
                s.name == "bslide"
            ) {

                brightnessSlider = s;
            }

            if (
                s.name == "satslide"
            ) {

                saturationSlider = s;
            }

            if (
                s.name == "vslide"
            ) {

                volumeSlider = s;
            }
        }

        if (
            brightnessSlider != null
        ) {

            brightnessSlider.onValueChanged
                .RemoveAllListeners();

            brightnessSlider.onValueChanged
                .AddListener(
                    SetBrightness
                );
        }

        if (
            saturationSlider != null
        ) {

            saturationSlider.onValueChanged
                .RemoveAllListeners();

            saturationSlider.onValueChanged
                .AddListener(
                    SetSaturation
                );
        }

        if (
            volumeSlider != null
        ) {

            volumeSlider.onValueChanged
                .RemoveAllListeners();

            volumeSlider.onValueChanged
                .AddListener(
                    SetVolume
                );
        }
    }

    void SetupVolume() {

        GameObject vol =
            GameObject.Find(
                "GlobalVolume"
            );

        if (
            vol != null
        ) {

            globalVolume =
                vol.GetComponent<
                    Volume
                >();
        }

        if (
            globalVolume != null
            &&
            globalVolume.profile != null
        ) {

            globalVolume.profile =
                Instantiate(
                    globalVolume.profile
                );

            globalVolume.profile.TryGet(
                out colorAdjustments
            );
        }
    }

    void SetupAudioSlider() {

        AudioListener.volume =
            volume;

        if (
            volumeSlider != null
        ) {

            volumeSlider.value =
                volume;
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