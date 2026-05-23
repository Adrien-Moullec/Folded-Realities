using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplaySystem : MonoBehaviour {

    public static GameplaySystem instance;
    [SerializeField] SceneTransition sceneTransition;
    [HideInInspector] public int slot = -1;
    public string currentSlotId => GetSlotID(slot);
    private string GetSlotID(int id) => "Slot" + id + "_";

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region Scene Management

    public void LoadSlot(int loadSlotId) {
        slot = loadSlotId;
    }
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
    #endregion

    #region Player Pref Data Management
    public void SaveSettings() =>
        PlayerPrefs.Save();
    public void DeleteSettings(int slotID) {
        foreach (var n in System.Enum.GetNames(typeof(PrefFloat)))
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString());
        foreach (var n in System.Enum.GetNames(typeof(PrefInt)))
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString());
        foreach (var n in System.Enum.GetNames(typeof(PrefString)))
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString());
        foreach (var n in System.Enum.GetNames(typeof(PrefVector3))) {
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString() + "-X");
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString() + "-Y");
            PlayerPrefs.DeleteKey(GetSlotID(slotID) + n.ToString() + "-Z");
        }
    }

    /// FLOAT
    public void SetFloat(PrefFloat prefFloat, float value, bool isSlotInfo = true) =>
        PlayerPrefs.SetFloat((isSlotInfo ? currentSlotId : "") + prefFloat.ToString(), value);
    public float GetFloat(PrefFloat prefFloat, float def = 0, bool isSlotInfo = true) =>
        PlayerPrefs.GetFloat((isSlotInfo ? currentSlotId : "") + prefFloat.ToString(), def);

    /// INTEGER
    public void SetInt(PrefInt prefInt, int value, bool isSlotInfo = true) =>
        PlayerPrefs.SetInt((isSlotInfo ? currentSlotId : "") + prefInt.ToString(), value);
    public int GetInt(PrefInt prefInt, int def = 0, bool isSlotInfo = true) =>
        PlayerPrefs.GetInt((isSlotInfo ? currentSlotId : "") + prefInt.ToString(), def);

    /// STRING
    public void SetString(PrefString prefString, string value, bool isSlotInfo = true) =>
        PlayerPrefs.SetString((isSlotInfo ? currentSlotId : "") + prefString.ToString(), value);
    public string GetString(PrefString prefString, string def = "", bool isSlotInfo = true) =>
        PlayerPrefs.GetString((isSlotInfo ? currentSlotId : "") + prefString.ToString(), def);

    /// VECTOR3
    public void SetVector3(PrefVector3 prefVec3, Vector3 value, bool isSlotInfo = true) {
        PlayerPrefs.SetFloat((isSlotInfo ? currentSlotId : "") + prefVec3.ToString() + "-X", value.x);
        PlayerPrefs.SetFloat((isSlotInfo ? currentSlotId : "") + prefVec3.ToString() + "-Y", value.y);
        PlayerPrefs.SetFloat((isSlotInfo ? currentSlotId : "") + prefVec3.ToString() + "-Z", value.z);
    }
    public Vector3 GetVector3(PrefVector3 prefVec3, Vector3 def, bool isSlotInfo = true) =>
        new Vector3(
            PlayerPrefs.GetFloat((isSlotInfo ? currentSlotId : "") + prefVec3.ToString() + "-X", def.x),
            PlayerPrefs.GetFloat((isSlotInfo ? currentSlotId : "") + prefVec3.ToString() + "-Y", def.y),
            PlayerPrefs.GetFloat((isSlotInfo ? currentSlotId : "") + prefVec3.ToString() + "-Z", def.z)
        );
    #endregion
}

#region Player Pref Summary
public enum PrefFloat {
    // Settings
    GameVolume,
    Brightness
}
public enum PrefInt {
    Progress,
    DoesSlotExist,
    Coins,
}
public enum PrefString {
    Time,
    Scene
}
public enum PrefVector3 {

}
public enum TransitionType {
    Iris
}
public enum GameplayScenes {
    MainMenu,
    Bedroom
}
#endregion