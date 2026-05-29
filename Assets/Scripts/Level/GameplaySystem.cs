using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base object for data and scene transitioning that carries over across the game from Main Menu.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class GameplaySystem : MonoBehaviour {

    [Tooltip("GameplaySystem instance to be called from any scene.")]
    public static GameplaySystem instance;
    [Tooltip("Scene transition shader object.")]
    [SerializeField] SceneTransition sceneTransition;
    [Tooltip("Pause menu reference.")]
    [SerializeField] PauseMenu pauseMenu;
    [Tooltip("Current slot id data.")]
    [HideInInspector] public static int slot = -1;
    [Tooltip("Get current slot ID string for player pref.")]
    public static string currentSlotId => GetSlotID(slot);
    [Tooltip("Get current slot ID string for player pref.")]
    private static string GetSlotID(int id) => "Slot" + id + "_";

    [Tooltip("Reset values when playing through level.")]
    public bool ResetOnPlay = true;

    [Tooltip("Player Input for activating menus.")]
    PlayerInput playerInput;
    [Tooltip("Player action for activating pause menu.")]
    InputAction pauseButton;

    [Tooltip("Setup singleton and reset playerprefs in editor mode.")]
    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        pauseMenu.gameObject.SetActive(false);
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

    /// <summary>
    /// Setup player input.
    /// </summary>
    void OnEnable() {
        playerInput = GetComponent<PlayerInput>();

        pauseButton = playerInput.actions["Pause"];
        pauseButton.performed += input => pauseMenu.TogglePause();
    }
    /// <summary>
    /// Disable player inputs
    /// </summary>
    private void OnDisable() {
        pauseButton.performed -= input => pauseMenu.TogglePause();
    }

    /// <summary>
    /// Load scenes based on transition types, scene enum input, current scene options and player situation.
    /// </summary>
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
    /// <summary>
    /// Save data.
    /// </summary>
    public static void SaveSettings() => PlayerPrefs.Save();

    /// <summary>
    /// Delete data in slot by looping through enums and deleting available data.
    /// </summary>
    /// <param name="slotID"> Slot ID to target. </param>
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

    /// SCENE SAVE POINT
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

    /// INDIVIDUAL OBJECT ID VISITED CHECK - collect stars or objectives only once
    public static void SetIdActive(int id, bool active) =>
        PlayerPrefs.SetInt(currentSlotId + id.ToString(), active ? 1 : 0);
    public static bool IsIdActive(int id) =>
        PlayerPrefs.GetInt(currentSlotId + id.ToString(), 1) == 1;
    #endregion
}

#if UNITY_EDITOR
/// <summary>
/// Tests for editor mode.
/// </summary>
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