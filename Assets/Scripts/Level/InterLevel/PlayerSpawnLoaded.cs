/*using UnityEngine;

public class PlayerSpawnLoaded : MonoBehaviour {

    void Start() {
 // Checks if saved spawn scene matches current scene
        if (PlayersPrefs.GetString("SpawnScene") == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) {
   // Loads saved player position
            float x = PlayersPrefs.GetFloat("SpawnX");
            float y = PlayersPrefs.GetFloat("SpawnY");
            float z = PlayersPrefs.GetFloat("SpawnZ");
  // Applies saved spawn position
            transform.position = new Vector3(x, y, z);
        }
    }
}*/