using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour {
    public static PauseMenu Instance;

    [Header("Pause")]
    public GameObject pauseMenu;
    public GameObject buttonsContainer;
    public GameObject settingsPanel;
    public GameObject savePanel;
    public GameObject overwritePanel;

    bool isPaused;

    [Header("Player")]
    public Transform player;
    public MonoBehaviour playerController;

    [Header("Progress")]
    public Vector3 lastCheckpoint;
    public int coins;

    [Header("UI Slots")]
    public TextMeshProUGUI slot1Text;
    public TextMeshProUGUI slot2Text;
    public TextMeshProUGUI slot3Text;

    int pendingSlot = -1;

    void Awake() {
        Instance = this;
    }

    void Start() {
        UpdateAllSlots();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        buttonsContainer.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerController != null)
            playerController.enabled = false;
    }

    public void Resume() {
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerController != null)
            playerController.enabled = true;
    }

    public void OpenSettings() {
        buttonsContainer.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenSavePanel() {
        buttonsContainer.SetActive(false);
        savePanel.SetActive(true);
    }

    public void BackToMain() {
        buttonsContainer.SetActive(true);

        settingsPanel.SetActive(false);
        savePanel.SetActive(false);
        overwritePanel.SetActive(false);
    }

    public void SetCheckpoint(Vector3 position) {
        lastCheckpoint = position;
    }

    public void AddCoins(int amount) {
        coins += amount;
    }

    public void SelectSlot(int slot) {
        if (PlayerPrefs.GetInt("Slot" + slot + "_Exists", 0) == 1) {
            pendingSlot = slot;
            overwritePanel.SetActive(true);
        } else {
            SaveGame(slot);
        }
    }

    public void ConfirmOverwrite() {
        SaveGame(pendingSlot);
        overwritePanel.SetActive(false);
        BackToMain();
        pendingSlot = -1;
    }

    public void CancelOverwrite() {
        overwritePanel.SetActive(false);
        pendingSlot = -1;
    }

    void SaveGame(int slot) {
        PlayerPrefs.SetFloat("Slot" + slot + "_X", lastCheckpoint.x);
        PlayerPrefs.SetFloat("Slot" + slot + "_Y", lastCheckpoint.y);
        PlayerPrefs.SetFloat("Slot" + slot + "_Z", lastCheckpoint.z);

        PlayerPrefs.SetInt("Slot" + slot + "_Coins", coins);
        PlayerPrefs.SetInt("Slot" + slot + "_Exists", 1);

        string time = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        PlayerPrefs.SetString("Slot" + slot + "_Time", time);

        PlayerPrefs.Save();

        UpdateSlotUI(slot);
    }

    void UpdateAllSlots() {
        UpdateSlotUI(1);
        UpdateSlotUI(2);
        UpdateSlotUI(3);
    }

    void UpdateSlotUI(int slot) {
        TextMeshProUGUI text = null;

        if (slot == 1) text = slot1Text;
        if (slot == 2) text = slot2Text;
        if (slot == 3) text = slot3Text;

        if (text == null) return;

        if (PlayerPrefs.GetInt("Slot" + slot + "_Exists", 0) == 1) {
            string time = PlayerPrefs.GetString("Slot" + slot + "_Time", "No Time");
            int savedCoins = PlayerPrefs.GetInt("Slot" + slot + "_Coins", 0);

            text.text = "Saved\n" + time + "\nCoins: " + savedCoins;
        } else {
            text.text = "Empty";
        }
    }
}