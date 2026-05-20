using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;

    public Slider volumeSlider;

    void Awake() {

        if (
            Instance != null
            && Instance != this
        ) {

            Destroy(
                gameObject
            );

            return;
        }

        Instance = this;

        DontDestroyOnLoad(
            gameObject
        );
    }

    void Start() {

        float savedVolume =
            PlayerPrefs.GetFloat(
                "GameVolume",
                1f
            );

        AudioListener.volume =
            savedVolume;

        if (
            volumeSlider != null
        ) {

            volumeSlider.value =
                savedVolume;

            volumeSlider.onValueChanged
                .AddListener(
                    SetVolume
                );
        }
    }

    public void SetVolume(
        float volume
    ) {

        AudioListener.volume =
            volume;

        PlayerPrefs.SetFloat(
            "GameVolume",
            volume
        );

        PlayerPrefs.Save();
    }
}