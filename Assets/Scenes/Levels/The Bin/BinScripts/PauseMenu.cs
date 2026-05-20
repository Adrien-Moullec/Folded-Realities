using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
using AbilitySystem;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu Instance;

    public GameObject pauseMenu;
    public GameObject buttonsContainer;
    public GameObject settingsPanel;
    public GameObject savePanel;
    public GameObject overwritePanel;
    public GameObject quitConfirmPanel;
    public GameObject backgroundPanel;

    public bool isPaused;

    public Transform player;


    public Vector3 lastCheckpoint;

    public int coins;

    public TextMeshProUGUI slot1Text;
    public TextMeshProUGUI slot2Text;
    public TextMeshProUGUI slot3Text;

    int pendingSlot = -1;

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

        pauseMenu.SetActive(false);

        buttonsContainer.SetActive(true);

        settingsPanel.SetActive(false);

        savePanel.SetActive(false);

        overwritePanel.SetActive(false);

        quitConfirmPanel.SetActive(false);

        backgroundPanel.SetActive(true);

        UpdateAllSlots();
    }

    void Update() {

        if (
            Input.GetKeyDown(
                KeyCode.P
            )
        ) {

            if (isPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    ) {

        GameObject p =
            GameObject.FindGameObjectWithTag(
                "Player"
            );

        if (p != null) {

            player =
                p.transform;

        }

        if (
            GameLoader.slotToLoad != -1
        ) {

            StartCoroutine(
                LoadAfterDelay(
                    GameLoader.slotToLoad
                )
            );

            GameLoader.slotToLoad = -1;
        }
    }

    IEnumerator LoadAfterDelay(
        int slot
    ) {

        yield return new WaitForSeconds(
            0.1f
        );

        DisablePlayerSystems();

        LoadGame(slot);

        yield return new WaitForSeconds(
            0.1f
        );

        SnapToGround();

        yield return new WaitForSeconds(
            0.1f
        );

        EnablePlayerSystems();

    }

    void DisablePlayerSystems() {

        if (
            player == null
        ) return;

        CharacterController cc =
            player.GetComponent<
                CharacterController
            >();

        if (
            cc != null
        ) {
            cc.enabled = false;
        }

        Rigidbody rb =
            player.GetComponent<
                Rigidbody
            >();

        if (
            rb != null
        ) {

            rb.linearVelocity =
                Vector3.zero;

            rb.angularVelocity =
                Vector3.zero;

            rb.isKinematic = true;
        }
    }

    void EnablePlayerSystems() {

        if (
            player == null
        ) return;

        CharacterController cc =
            player.GetComponent<
                CharacterController
            >();

        if (
            cc != null
        ) {
            cc.enabled = true;
        }

        Rigidbody rb =
            player.GetComponent<
                Rigidbody
            >();

        if (
            rb != null
        ) {

            rb.isKinematic = false;
        }
    }

    void SnapToGround() {

        if (
            player == null
        ) return;

        RaycastHit hit;

        if (
            Physics.Raycast(
                player.position + Vector3.up,
                Vector3.down,
                out hit,
                20f
            )
        ) {

            player.position =
                hit.point;
        }
    }

    public void Pause() {

        pauseMenu.SetActive(true);

        buttonsContainer.SetActive(true);

        settingsPanel.SetActive(false);

        savePanel.SetActive(false);

        overwritePanel.SetActive(false);

        quitConfirmPanel.SetActive(false);

        backgroundPanel.SetActive(true);

        Time.timeScale = 0f;

        isPaused = true;

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        UnityEngine.EventSystems
            .EventSystem.current
            .SetSelectedGameObject(
                null
            );

        DisablePlayerSystems();
    }

    public void Resume() {

        pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        isPaused = false;

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
        Cursor.lockState =
       CursorLockMode.Locked;

        Cursor.visible =
            false;

        EnablePlayerSystems();
    }

    public void OpenSettings() {

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        backgroundPanel.SetActive(false);

        buttonsContainer.SetActive(false);

        settingsPanel.SetActive(true);

        if (
            GraphicsSettings.Instance
            != null
        ) {

            GraphicsSettings.Instance
                .CacheCurrentSettings();
        }
    }

    public void OpenSavePanel() {

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        backgroundPanel.SetActive(true);

        buttonsContainer.SetActive(false);

        savePanel.SetActive(true);
    }

    public void OpenQuitConfirm() {

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        buttonsContainer.SetActive(false);

        quitConfirmPanel.SetActive(true);
    }

    public void CancelQuit() {

        quitConfirmPanel.SetActive(false);

        buttonsContainer.SetActive(true);
    }

    public void QuitToMainMenuConfirmed() {

        Time.timeScale = 1f;

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        SceneManager.LoadScene(
            "MainMenu"
        );
    }

    public void BackToMain() {

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        backgroundPanel.SetActive(true);

        buttonsContainer.SetActive(true);

        settingsPanel.SetActive(false);

        savePanel.SetActive(false);

        overwritePanel.SetActive(false);

        quitConfirmPanel.SetActive(false);
    }

    public void SetCheckpoint(
        Vector3 position
    ) {

        lastCheckpoint =
            position;
    }

    public void AddCoins(
        int amount
    ) {

        coins += amount;
    }

    public void SelectSlot(
        int slot
    ) {

        if (
            PlayerPrefs.GetInt(
                "Slot" + slot + "_Exists",
                0
            ) == 1
        ) {

            pendingSlot = slot;

            overwritePanel.SetActive(
                true
            );
        } else {

            SaveGame(slot);
        }
    }

    public void ConfirmOverwrite() {

        SaveGame(
            pendingSlot
        );

        overwritePanel.SetActive(
            false
        );

        pendingSlot = -1;
    }

    public void CancelOverwrite() {

        overwritePanel.SetActive(
            false
        );

        pendingSlot = -1;
    }

    void SaveGame(
        int slot
    ) {

        Vector3 pos =
            player != null
            ? player.position
            : lastCheckpoint;

        PlayerPrefs.SetFloat(
            "Slot" + slot + "_X",
            pos.x
        );

        PlayerPrefs.SetFloat(
            "Slot" + slot + "_Y",
            pos.y
        );

        PlayerPrefs.SetFloat(
            "Slot" + slot + "_Z",
            pos.z
        );

        PlayerPrefs.SetInt(
            "Slot" + slot + "_Coins",
            coins
        );

        PlayerPrefs.SetString(
            "Slot" + slot + "_Scene",
            SceneManager
                .GetActiveScene()
                .name
        );

        PlayerPrefs.SetInt(
            "Slot" + slot + "_Exists",
            1
        );

        string time =
            System.DateTime.Now.ToString(
                "dd/MM/yyyy HH:mm"
            );

        PlayerPrefs.SetString(
            "Slot" + slot + "_Time",
            time
        );

        PlayerPrefs.Save();

        UpdateSlotUI(slot);
    }

    public void LoadGame(
        int slot
    ) {

        if (
            PlayerPrefs.GetInt(
                "Slot" + slot + "_Exists",
                0
            ) != 1
        ) return;

        float x =
            PlayerPrefs.GetFloat(
                "Slot" + slot + "_X",
                0f
            );

        float y =
            PlayerPrefs.GetFloat(
                "Slot" + slot + "_Y",
                0f
            );

        float z =
            PlayerPrefs.GetFloat(
                "Slot" + slot + "_Z",
                0f
            );

        coins =
            PlayerPrefs.GetInt(
                "Slot" + slot + "_Coins",
                0
            );

        Vector3 loadPos =
            new Vector3(
                x,
                y,
                z
            );

        if (
            loadPos == Vector3.zero
        ) {

            GameObject start =
                GameObject.FindGameObjectWithTag(
                    "PlayerStart"
                );

            if (
                start != null
            ) {

                loadPos =
                    start.transform.position;
            }
        }

        if (
            player != null
        ) {

            player.position =
                loadPos;
        }

        lastCheckpoint =
            loadPos;
    }

    void UpdateAllSlots() {

        UpdateSlotUI(1);

        UpdateSlotUI(2);

        UpdateSlotUI(3);
    }

    void UpdateSlotUI(
        int slot
    ) {

        TextMeshProUGUI text =
            null;

        if (slot == 1)
            text = slot1Text;

        if (slot == 2)
            text = slot2Text;

        if (slot == 3)
            text = slot3Text;

        if (
            text == null
        ) return;

        if (
            PlayerPrefs.GetInt(
                "Slot" + slot + "_Exists",
                0
            ) == 1
        ) {

            string time =
                PlayerPrefs.GetString(
                    "Slot" + slot + "_Time",
                    "No Time"
                );

            int savedCoins =
                PlayerPrefs.GetInt(
                    "Slot" + slot + "_Coins",
                    0
                );

            string scene =
                PlayerPrefs.GetString(
                    "Slot" + slot + "_Scene",
                    "Unknown"
                );

            text.text =
                "Saved\n"
                + scene
                + "\n"
                + time
                + "\nCoins: "
                + savedCoins;
        } else {

            text.text =
                "Empty";
        }
    }
}