using UnityEngine;

public class PlayerSpawnLoaded : MonoBehaviour {

    void Start() {

        if (PlayerPrefs.GetString("SpawnScene") == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) {
            float x = PlayerPrefs.GetFloat("SpawnX");
            float y = PlayerPrefs.GetFloat("SpawnY");
            float z = PlayerPrefs.GetFloat("SpawnZ");

            transform.position = new Vector3(x, y, z);
        }
    }
}