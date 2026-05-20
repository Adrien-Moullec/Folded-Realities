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

    float originalBrightness;

    float originalSaturation;

    [Header("Audio")]
    public Slider volumeSlider;

    float volume;

    void Awake() {

        if (
            Instance == null
        ) {

            Instance = this;

            DontDestroyOnLoad(
                gameObject
            );
        } else {

            Destroy(
                gameObject
            );
        }
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

        PlayerPrefs.DeleteKey(
            "GameVolume"
        );

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

        SetupVolume();

        ApplySettings();

        UpdateSliders();

        SetupAudioSlider();
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

            volumeSlider.onValueChanged
                .RemoveAllListeners();

            volumeSlider.onValueChanged
                .AddListener(
                    SetVolume
                );
        }
    }

    public void CacheCurrentSettings() {

        originalBrightness =
            brightness;

        originalSaturation =
            saturation;
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

        PlayerPrefs.SetFloat(
            "GameVolume",
            volume
        );

        PlayerPrefs.Save();
    }

    public void ConfirmSettings() {

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

    public void RevertSettings() {

        brightness =
            originalBrightness;

        saturation =
            originalSaturation;

        UpdateSliders();

        ApplySettings();
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