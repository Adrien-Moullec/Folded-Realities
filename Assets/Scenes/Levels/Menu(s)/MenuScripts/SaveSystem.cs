using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour {
    public static SaveSystem Instance;

    void Awake() {
        Instance = this;
    }

    public void SaveGame() {
        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
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