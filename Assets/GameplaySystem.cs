using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(PlayerInput))]
public class GameplaySystem : MonoBehaviour {

    public static GameplaySystem instance;
    [SerializeField] SceneTransition sceneTransition;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GraphicsSettings graphicsSettings;
    [HideInInspector] TargetLevel targetLevel;
    [HideInInspector] public static int slot = -1;
    public static string currentSlotId => GetSlotID(slot);
    private static string GetSlotID(int id) => "Slot" + id + "_";
    public bool ResetOnPlay = true;
    private bool pauseMenuActive = false;
    PlayerInput playerInput;
    InputAction pauseButton;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        pauseMenu.SetActive(false);
#if UNITY_EDITOR
        if (ResetOnPlay) {
            DeleteSettings(-1);
            DeleteSettings(1);
            DeleteSettings(2);
            DeleteSettings(3);
        }
#endif
        sceneTransition.gameObject.SetActive(true);
    }
    void OnEnable() {
        playerInput = GetComponent<PlayerInput>();

        pauseButton = playerInput.actions["Pause"];
        pauseButton.performed += input => OnPauseMenu();
    }

    public void OnPauseMenu() {
        switch (SceneManager.GetActiveScene().name) {
            case nameof(GameplayScenes.BossCutscene): return;
            case nameof(GameplayScenes.END): return;
            case nameof(GameplayScenes.IntroCutscene): return;
            case nameof(GameplayScenes.MainMenu): return;
        }
        bool turnOnMenu = !pauseMenu.activeSelf;
        pauseMenu.SetActive(turnOnMenu);
        if (turnOnMenu) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Time.timeScale = turnOnMenu ? 0 : 1;
        Cursor.lockState = turnOnMenu ? CursorLockMode.None : CursorLockMode.Locked;
    }

    #region Settings
    public void SetVolume(float value) => SetSettingsFloat(SettingsFloatPref.GameVolume, value);
    public void SetBrightness(float value) => SetSettingsFloat(SettingsFloatPref.Brightness, value);
    public void SetSaturation(float value) => SetSettingsFloat(SettingsFloatPref.Saturation, value);
    #endregion

    #region Scene Management
    public void LoadScene(TargetLevel scene, TransitionType transition = TransitionType.Iris) {
        PlayerPrefs.Save();
        sceneTransition?.TransitionToScene(scene.targetScene.ToString(), scene.targetId);
    }
    public void LoadScene(GameplayScenes scene, TransitionType transition = TransitionType.Iris) {
        PlayerPrefs.Save();
        sceneTransition?.TransitionToScene(scene.ToString(), -1);
    }
    public IEnumerator Respawn(bool savePoint = true, TransitionType transition = TransitionType.Iris) {
        PlayerPrefs.Save();
        yield return sceneTransition?.RespawnTransition();
    }
    public IEnumerator BossTransition(GameplayScenes scene, TransitionType transition = TransitionType.Iris) {
        PlayerPrefs.Save();
        yield return sceneTransition?.BossDeathTransition();
        SceneManager.LoadScene(scene.ToString());
    }

    public void StartGame(int slotId) {
        slot = slotId;
        PlayerPrefs.Save();
        sceneTransition?.TransitionToScene(GetString(PrefString.SavedScene, GameplayScenes.IntroCutscene.ToString()), -1);
    }
    public void SetCurrentSaveScene(GameplayScenes gameplayScenes) {
        SetString(PrefString.SavedScene, gameplayScenes.ToString());
    }
    public void Quit() {
        if (SceneManager.GetActiveScene().name == GameplayScenes.MainMenu.ToString()) {
            SaveSettings();
            Application.Quit();//
        } else {
            LoadScene(GameplayScenes.MainMenu, TransitionType.Iris);
        }
    }
    #endregion

    #region Player Pref Data Management
    public static void SaveSettings() =>
        PlayerPrefs.Save();
    public static void DeleteSettings(int slotID) {
        string key = "";
        foreach (var n in Enum.GetNames(typeof(PrefInt)))
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString());
        foreach (var n in Enum.GetNames(typeof(PrefString)))
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString());
        foreach (var n in Enum.GetNames(typeof(PrefVector3))) {
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString() + "-X");
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString() + "-Y");
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString() + "-Z");
        }
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            GetSlotID(slotID);

            foreach (var obj in FindObjectsByType<PlayerPrefIDGenerator>(FindObjectsSortMode.None)) {
                key = GetSlotID(slotID) + PlayerPrefIDGenerator.GetIdGeneration(obj.gameObject, sceneName);
                if (PlayerPrefs.GetInt(key, -1) == 1) Debug.Log("Found one at " + sceneName);
                PlayerPrefs.DeleteKey(key);
            }
        }
        SaveSettings();
    }


    /// FLOAT
    public static void SetSettingsFloat(SettingsFloatPref key, float value) {
        PlayerPrefs.SetFloat(key.ToString(), value);
        switch (key) {
            case SettingsFloatPref.Brightness: return;
            case SettingsFloatPref.GameVolume: return;
            case SettingsFloatPref.Saturation: return;
        }
    }
    public static float GetSettingsFloat(SettingsFloatPref key, float def = 0) =>
        PlayerPrefs.GetFloat(key.ToString(), def);

    /// INTEGER
    public static void SetInt(PrefInt key, int value, bool isSlotInfo = true) =>
        PlayerPrefs.SetInt((isSlotInfo ? currentSlotId : "") + key.ToString(), value);
    public static int GetInt(PrefInt key, int def = 0, bool isSlotInfo = true) =>
        PlayerPrefs.GetInt((isSlotInfo ? currentSlotId : "") + key.ToString(), def);

    /// STRING
    public static void SetString(PrefString key, string value, bool isSlotInfo = true) =>
        PlayerPrefs.SetString((isSlotInfo ? currentSlotId : "") + key.ToString(), value);
    public static string GetString(PrefString key, string def = "", bool isSlotInfo = true) =>
        PlayerPrefs.GetString((isSlotInfo ? currentSlotId : "") + key.ToString(), def);

    /// VECTOR3
    public static void SetVector3(PrefVector3 key, Vector3 value, bool isSlotInfo = true) {
        PlayerPrefs.SetFloat((isSlotInfo ? currentSlotId : "") + key.ToString() + "-X", value.x);
        PlayerPrefs.SetFloat((isSlotInfo ? currentSlotId : "") + key.ToString() + "-Y", value.y);
        PlayerPrefs.SetFloat((isSlotInfo ? currentSlotId : "") + key.ToString() + "-Z", value.z);
    }
    public static Vector3 GetVector3(PrefVector3 key, Vector3 def, bool isSlotInfo = true) =>
        new Vector3(
            PlayerPrefs.GetFloat((isSlotInfo ? currentSlotId : "") + key.ToString() + "-X", def.x),
            PlayerPrefs.GetFloat((isSlotInfo ? currentSlotId : "") + key.ToString() + "-Y", def.y),
            PlayerPrefs.GetFloat((isSlotInfo ? currentSlotId : "") + key.ToString() + "-Z", def.z)
        );
    public static void SetSceneSavePoint(string scene, Vector3 value) {
        PlayerPrefs.SetFloat(currentSlotId + scene + "-X", value.x);
        PlayerPrefs.SetFloat(currentSlotId + scene + "-Y", value.y);
        PlayerPrefs.SetFloat(currentSlotId + scene + "-Z", value.z);
    }
    public static Vector3 GetSceneSavePoint(string scene, Vector3 def)
        => new Vector3(
            PlayerPrefs.GetFloat(currentSlotId + scene + "-X", def.x),
            PlayerPrefs.GetFloat(currentSlotId + scene + "-Y", def.y),
            PlayerPrefs.GetFloat(currentSlotId + scene + "-Z", def.z)
        );

    public static void SetIdActive(int id, bool active) =>
        PlayerPrefs.SetInt(currentSlotId + id.ToString(), active ? 1 : 0);
    public static bool IsIdActive(int id) =>
        PlayerPrefs.GetInt(currentSlotId + id.ToString(), 1) == 1;

    internal static void SetBool() {
        throw new NotImplementedException();
    }
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameplaySystem))]
[CanEditMultipleObjects]
public class GameplaySystemEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("ResetPlayerPrefs")) {
            GameplaySystem.DeleteSettings(-1);
            GameplaySystem.DeleteSettings(1);
            GameplaySystem.DeleteSettings(2);
            GameplaySystem.DeleteSettings(3);
        }
    }
}
#endif