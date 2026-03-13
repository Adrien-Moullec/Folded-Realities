using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevelSpawn : MonoBehaviour {
    void Start() {
        string savedScene = PlayerPrefs.GetString("SpawnScene", "");

        if (savedScene == SceneManager.GetActiveScene().name) {
            float x = PlayerPrefs.GetFloat("SpawnX");
            float y = PlayerPrefs.GetFloat("SpawnY");
            float z = PlayerPrefs.GetFloat("SpawnZ");

            transform.position = new Vector3(x, y, z);

            PlayerPrefs.DeleteKey("SpawnScene");
        }
    }
}