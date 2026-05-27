using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.EventSystems;
using AbilitySystem;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu Instance;

    #region Panels

    [Header("Panels")]
    public GameObject pauseMenu;
    public GameObject buttonsContainer;
    public GameObject settingsPanel;
    public GameObject savePanel;
    public GameObject overwritePanel;
    public GameObject quitConfirmPanel;
    public GameObject backgroundPanel;

    #endregion

    #region Player

    [Header("Player")]
    public Transform player;

    #endregion

    #region Save Slots

    [Header("Save Slots")]
    public TextMeshProUGUI slot1Text;
    public TextMeshProUGUI slot2Text;
    public TextMeshProUGUI slot3Text;

    #endregion

    #region Variables

    public bool isPaused;

    public Vector3 lastCheckpoint;

    public static int coins;

    int pendingSlot = -1;

    #endregion

    void Awake() {

        Instance = this;
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

        overwritePanel?.SetActive(false);

        quitConfirmPanel.SetActive(false);

        backgroundPanel.SetActive(true);

        UpdateAllSlots();
    }
    /*
        void Update() {

            // Toggle pause menu
            if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame) {

                if (isPaused)
                    Resume();
                else
                    Pause();
            }
        }*/

    #region Scene Loading

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
            player = p.transform;

        StartCoroutine(LoadAfterDelay(GameplaySystem.slot));
    }
    public void TogglePause() {
        if (isPaused)
            Resume();
        else
            Pause();
    }
    IEnumerator LoadAfterDelay(int slot) {

        if (slot == -1)
            yield break;

        yield return new WaitForSeconds(0.1f);

        DisablePlayerSystems();

        LoadGame(slot);

        yield return new WaitForSeconds(0.1f);

        SnapToGround();

        yield return new WaitForSeconds(0.1f);

        EnablePlayerSystems();
    }

    #endregion

    #region Player Systems

    void DisablePlayerSystems() {

        if (player == null)
            return;

        if (player.TryGetComponent(out CharacterController cc))
            cc.enabled = false;

        if (player.TryGetComponent(out Rigidbody rb)) {

            rb.linearVelocity = Vector3.zero;

            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
        }
    }

    void EnablePlayerSystems() {

        if (player == null)
            return;

        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
            cc.enabled = true;

        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
            rb.isKinematic = false;
    }

    void SnapToGround() {

        // Prevents floating after loading
        if (player == null)
            return;

        if (Physics.Raycast(player.position + Vector3.up, Vector3.down, out RaycastHit hit, 20f))
            player.position = hit.point;
    }

    #endregion

    #region Pause Controls

    public void Pause() {

        pauseMenu.SetActive(true);

        buttonsContainer.SetActive(true);

        settingsPanel.SetActive(false);

        savePanel.SetActive(false);

        overwritePanel?.SetActive(false);

        quitConfirmPanel.SetActive(false);

        backgroundPanel.SetActive(true);

        Time.timeScale = 0f;

        isPaused = true;

        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;

        EventSystem.current?.SetSelectedGameObject(null);

        DisablePlayerSystems();
    }

    public void Resume() {

        pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        EnablePlayerSystems();
    }

    #endregion

    #region Panels

    public void OpenSettings() {

        backgroundPanel.SetActive(false);

        buttonsContainer.SetActive(false);

        settingsPanel.SetActive(true);

        if (GraphicsSettings.Instance != null)
            GraphicsSettings.Instance.CacheCurrentSettings();
    }

    public void CloseSettings() {

        if (GraphicsSettings.Instance != null)
            GraphicsSettings.Instance.SaveSettings();

        BackToMain();
    }

    public void RevertSettings() {

        if (GraphicsSettings.Instance != null)
            GraphicsSettings.Instance.RevertCachedSettings();
    }

    public void OpenSavePanel() {

        backgroundPanel.SetActive(true);

        buttonsContainer.SetActive(false);

        savePanel.SetActive(true);
    }

    public void OpenQuitConfirm() {

        buttonsContainer.SetActive(false);

        quitConfirmPanel.SetActive(true);
    }

    public void CancelQuit() {

        quitConfirmPanel.SetActive(false);

        buttonsContainer.SetActive(true);
    }

    public void BackToMain() {

        backgroundPanel.SetActive(true);

        buttonsContainer.SetActive(true);

        settingsPanel.SetActive(false);

        savePanel.SetActive(false);

        overwritePanel?.SetActive(false);

        quitConfirmPanel.SetActive(false);
    }

    #endregion

    #region Save System

    public void SetCheckpoint(Vector3 position) {

        lastCheckpoint = position;
    }

    public void AddCoins(int amount) {

        coins += amount;
    }

    public void SelectSlot(int slot) {

        // Opens overwrite confirmation
        if (GameplaySystem.GetInt(PrefInt.DoesSlotExist, 0) == 1) {

            pendingSlot = slot;

            overwritePanel?.SetActive(true);

        } else {

            SaveGame(slot);
        }
    }

    public void ConfirmOverwrite() {

        SaveGame(pendingSlot);

        overwritePanel?.SetActive(false);

        pendingSlot = -1;
    }

    public void CancelOverwrite() {

        overwritePanel?.SetActive(false);

        pendingSlot = -1;
    }

    public void ClearSlot(int slot) {

        GameplaySystem.DeleteSettings(slot);

        UpdateSlotUI(slot);
    }

    void SaveGame(int slot) {

        // Saves player progress
        Vector3 pos = player != null ? player.position : lastCheckpoint;

        GameplaySystem.SetVector3(PrefVector3.SavedLocation, pos);

        GameplaySystem.SetInt(PrefInt.Coins, coins);

        GameplaySystem.SetString(PrefString.SavedScene, SceneManager.GetActiveScene().name);

        GameplaySystem.SetInt(PrefInt.DoesSlotExist, 1);

        string time = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        GameplaySystem.SetString(PrefString.Time, time);

        GameplaySystem.SaveSettings();

        UpdateSlotUI(slot);
    }

    public void LoadGame(int slot) {

        if (GameplaySystem.GetInt(PrefInt.DoesSlotExist, 0) != 1)
            return;

        GameplaySystem.slot = slot;

        coins = GameplaySystem.GetInt(PrefInt.Coins, 0);

        string scene = GameplaySystem.GetString(PrefString.SavedScene, "Unknown");

        // Loads correct scene if needed
        if (SceneManager.GetActiveScene().name != scene) {

            GameplaySystem.instance.StartGame(slot);

            return;
        }

        Vector3 loadPos = GameplaySystem.GetVector3(PrefVector3.SavedLocation, Vector3.zero);

        if (loadPos == Vector3.zero) {

            GameObject start = GameObject.FindGameObjectWithTag("PlayerStart");

            if (start != null)
                loadPos = start.transform.position;
        }

        if (player != null)
            player.position = loadPos;

        lastCheckpoint = loadPos;
    }

    #endregion

    #region Quit

    public void QuitToMainMenuConfirmed() {

        Time.timeScale = 1f;

        isPaused = false;

        pauseMenu.SetActive(false);

        // Cleans up gameplay systems
        GameObject systems = GameObject.Find("GameplaySystems");

        if (systems != null)
            Destroy(systems);

        SceneManager.LoadScene("MainMenu");
    }

    #endregion

    #region Slot UI

    void UpdateAllSlots() {

        UpdateSlotUI(1);

        UpdateSlotUI(2);

        UpdateSlotUI(3);
    }

    void UpdateSlotUI(int slot) {

        TextMeshProUGUI text = null;

        if (slot == 1)
            text = slot1Text;

        if (slot == 2)
            text = slot2Text;

        if (slot == 3)
            text = slot3Text;

        if (text == null)
            return;

        // Updates save slot information
        if (GameplaySystem.GetInt(PrefInt.DoesSlotExist, 0) != 1) {

            string time = GameplaySystem.GetString(PrefString.Time, "No Time");

            int savedCoins = GameplaySystem.GetInt(PrefInt.Coins, 0);

            string scene = GameplaySystem.GetString(PrefString.SavedScene, "Unknown");

            text.text = "Saved\n" + scene + "\n" + time + "\nCoins: " + savedCoins;

        } else {

            text.text = "Empty";
        }
    }

    #endregion
}