using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

using System;

public class MainMenu : MonoBehaviour {

    public GameObject menuContainer;
    public GameObject loadGamePanel;
    public GameObject optionsPanel;
    public GameObject brightnessPanel;

    public TextMeshProUGUI slot1Text;
    public TextMeshProUGUI slot2Text;
    public TextMeshProUGUI slot3Text;

    LoadStyle loadStyle;
    public void SetNewGame() => loadStyle = LoadStyle.NewGame;
    public void SetLoadGame() => loadStyle = LoadStyle.LoadGame;

    void Start() {
        menuContainer.SetActive(true);
        loadGamePanel.SetActive(false);
        optionsPanel.SetActive(false);
        brightnessPanel.SetActive(false);

        UpdateSlotUI(1);
        UpdateSlotUI(2);
        UpdateSlotUI(3);
    }

    public void OpenLoadGame() {
        loadGamePanel.SetActive(true);
        loadGamePanel.transform.SetAsLastSibling();
    }

    public void CloseLoadGame() {
        loadGamePanel.SetActive(false);
    }

    public void LoadSlot(int slot) {
        switch (loadStyle) {
            case LoadStyle.NewGame: GameplaySystem.DeleteSettings(slot); break;
            case LoadStyle.LoadGame: break;
        }
        GameplaySystem.instance.StartGame(slot);
    }

    public void OpenOptions() {
        optionsPanel.SetActive(true);
        optionsPanel.transform.SetAsLastSibling();
    }

    public void CloseOptions() {
        optionsPanel.SetActive(false);
    }

    public void OpenBrightness() {
        brightnessPanel.SetActive(true);
        brightnessPanel.transform.SetAsLastSibling();
    }

    public void CloseBrightness() {
        brightnessPanel.SetActive(false);
    }

    public void ApplySettings() {
        GameplaySystem.SaveSettings();
        optionsPanel.SetActive(false);
        brightnessPanel.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }

    void UpdateSlotUI(int slot) {

        TextMeshProUGUI text = null;

        if (slot == 1) text = slot1Text;
        if (slot == 2) text = slot2Text;
        if (slot == 3) text = slot3Text;

        if (text == null) return;

        if (GameplaySystem.GetInt(PrefInt.DoesSlotExist, -1) == 1) {
            string time = GameplaySystem.GetString(PrefString.Time, "No Time");
            int coins = GameplaySystem.GetInt(PrefInt.Coins, 0);
            string scene = GameplaySystem.GetString(PrefString.SavedScene, "Unknown");

            text.text = "Saved\n" + scene + "\n" + time + "\nCoins: " + coins;

        } else {
            text.text = "Empty";
        }
    }
    public enum LoadStyle {
        NewGame,
        LoadGame
    }
}
