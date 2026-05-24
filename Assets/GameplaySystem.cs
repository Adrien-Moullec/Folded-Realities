using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplaySystem : MonoBehaviour {

    public static GameplaySystem instance;
    [SerializeField] SceneTransition sceneTransition;
    [HideInInspector] TargetLevel targetLevel;
    [HideInInspector] public static int slot = -1;
    public static string currentSlotId => GetSlotID(slot);
    private static string GetSlotID(int id) => "Slot" + id + "_";

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region Scene Management
    public void LoadScene(GameplayScenes scene, TransitionType transition) {
        PlayerPrefs.Save();
        sceneTransition?.TransitionToScene(scene.ToString());
    }
    public IEnumerator Respawn(GameObject player, TransitionType transition) {
        PlayerPrefs.Save();
        yield return sceneTransition?.RespawnTransition(player);
    }
    public IEnumerator BossTransition(GameplayScenes scene, TransitionType transition) {
        PlayerPrefs.Save();
        yield return sceneTransition?.BossDeathTransition();
        SceneManager.LoadScene(scene.ToString());
    }
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public void StartGame(int slotId) {
        slot = slotId;
        PlayerPrefs.Save();
        sceneTransition?.TransitionToScene(GetString(PrefString.SavedScene, GameplayScenes.IntroCutscene.ToString()));
    }
    public void SetCurrentSaveScene(GameplayScenes gameplayScenes) {
        SetString(PrefString.SavedScene, gameplayScenes.ToString());
    }
    #endregion

    #region Player Pref Data Management
    public static void SaveSettings() =>
        PlayerPrefs.Save();
    public static void DeleteSettings(int slotID) {
        foreach (var n in Enum.GetNames(typeof(PrefFloat)))
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString());
        foreach (var n in Enum.GetNames(typeof(PrefInt)))
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString());
        foreach (var n in Enum.GetNames(typeof(PrefString)))
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString());
        foreach (var n in Enum.GetNames(typeof(PrefVector3))) {
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString() + "-X");
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString() + "-Y");
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString() + "-Z");
        }
        SaveSettings();
    }

    /// FLOAT
    public static void SetFloat(PrefFloat key, float value, bool isSlotInfo = true) =>
        PlayerPrefs.SetFloat((isSlotInfo ? currentSlotId : "") + key.ToString(), value);
    public static float GetFloat(PrefFloat key, float def = 0, bool isSlotInfo = true) =>
        PlayerPrefs.GetFloat((isSlotInfo ? currentSlotId : "") + key.ToString(), def);

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
    #endregion
}
