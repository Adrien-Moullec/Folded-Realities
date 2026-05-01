using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu Instance;

    [Header("Pause")]
    public GameObject pauseMenu;
    public GameObject buttonsContainer;
    public GameObject settingsPanel;
    public GameObject savePanel;
    public GameObject overwritePanel;
    public GameObject backgroundPanel;

    public bool isPaused;

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

        pauseMenu.SetActive(false);

        buttonsContainer.SetActive(true);
        settingsPanel.SetActive(false);
        savePanel.SetActive(false);
        overwritePanel.SetActive(false);
        backgroundPanel.SetActive(true);

        UpdateAllSlots();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) Resume();
            else Pause();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) {
            player = p.transform;
        }

        if (GameLoader.slotToLoad != -1) {
            LoadGame(GameLoader.slotToLoad);
            GameLoader.slotToLoad = -1;
        }
    }

    public void Pause() {

        pauseMenu.SetActive(true);
        buttonsContainer.SetActive(true);

        settingsPanel.SetActive(false);
        savePanel.SetActive(false);
        overwritePanel.SetActive(false);

        backgroundPanel.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerController != null) {
            playerController.enabled = false;
        }
    }

    public void Resume() {

        pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerController != null) {
            playerController.enabled = true;
        }
    }

    public void OpenSettings() {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        backgroundPanel.SetActive(false);

        buttonsContainer.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenSavePanel() {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        backgroundPanel.SetActive(true);

        buttonsContainer.SetActive(false);
        savePanel.SetActive(true);
    }

    public void BackToMain() {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        backgroundPanel.SetActive(true);

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

    public void LoadGame(int slot) {

        if (PlayerPrefs.GetInt("Slot" + slot + "_Exists", 0) != 1) return;

        float x = PlayerPrefs.GetFloat("Slot" + slot + "_X");
        float y = PlayerPrefs.GetFloat("Slot" + slot + "_Y");
        float z = PlayerPrefs.GetFloat("Slot" + slot + "_Z");

        coins = PlayerPrefs.GetInt("Slot" + slot + "_Coins", 0);

        Vector3 loadPos = new Vector3(x, y, z);

        if (player != null) {
            player.position = loadPos;
        }
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