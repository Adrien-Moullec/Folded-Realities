using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour {
    public static SaveSystem Instance;

    void Awake() {
        Instance = this;
    }

    public void SaveGame() {
        GameplaySystem.SetString(PrefString.SavedScene, SceneManager.GetActiveScene().name);
        GameplaySystem.SaveSettings();
    }

    public void LoadGame() {
        if (PlayerPrefs.HasKey("SavedScene")) {
            string scene = PlayerPrefs.GetString("SavedScene");
            SceneManager.LoadScene(scene);
        } else {
            Debug.Log("No save found");
        }
    }
}