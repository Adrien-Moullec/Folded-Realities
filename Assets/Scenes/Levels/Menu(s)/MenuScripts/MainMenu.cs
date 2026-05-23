using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class MainMenu : MonoBehaviour {

    public GameObject menuContainer;
    public GameObject loadGamePanel;
    public GameObject optionsPanel;
    public GameObject brightnessPanel;

    public TextMeshProUGUI slot1Text;
    public TextMeshProUGUI slot2Text;
    public TextMeshProUGUI slot3Text;

    void Start() {
        menuContainer.SetActive(true);
        loadGamePanel.SetActive(false);
        optionsPanel.SetActive(false);
        brightnessPanel.SetActive(false);

        UpdateSlotUI(1);
        UpdateSlotUI(2);
        UpdateSlotUI(3);
    }

    public void StartGame() {
        SceneManager.LoadScene("IntroCutscene");
    }

    public void OpenLoadGame() {
        loadGamePanel.SetActive(true);
        loadGamePanel.transform.SetAsLastSibling();
    }

    public void CloseLoadGame() {
        loadGamePanel.SetActive(false);
    }

    public void LoadSlot1() {
        GameplaySystem.instance.LoadSlot(1);
    }

    public void LoadSlot2() {
        GameplaySystem.instance.LoadSlot(2);
    }

    public void LoadSlot3() {
        GameplaySystem.instance.LoadSlot(3);
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
        GameplaySystem.instance.SaveSettings();
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

        if (GameplaySystem.instance.GetInt(PrefInt.DoesSlotExist, -1) == 1) {
            string time = GameplaySystem.instance.GetString(PrefString.Time, "No Time");
            int coins = GameplaySystem.instance.GetInt(PrefInt.Coins, 0);
            string scene = GameplaySystem.instance.GetString(PrefString.Scene, "Unknown");

            text.text = "Saved\n" + scene + "\n" + time + "\nCoins: " + coins;

        } else {
            text.text = "Empty";
        }
    }
}