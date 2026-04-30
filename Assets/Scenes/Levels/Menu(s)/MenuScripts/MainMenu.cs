using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject menuContainer;
    public GameObject optionsPanel;
    public GameObject brightnessPanel;

    public void StartGame() {
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadGame() {
        if (SaveSystem.Instance != null) {
            SaveSystem.Instance.LoadGame();
        }
    }

    public void OpenOptions() {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions() {
        optionsPanel.SetActive(false);
        menuContainer.SetActive(true);
    }

    public void OpenBrightness() {
        optionsPanel.SetActive(false);
        brightnessPanel.SetActive(true);
    }

    public void CloseBrightness() {
        brightnessPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void ApplySettings() {
        PlayerPrefs.Save();
        CloseOptions();
    }

    public void ExitGame() {
        Application.Quit();
    }
}