using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevelSpawn : MonoBehaviour {
    void Start() {
        StartCoroutine(ApplySpawn());
    }

    IEnumerator ApplySpawn() {
        yield return null;
        yield return null;

        string savedScene = PlayerPrefs.GetString("SpawnScene", "");

        if (savedScene == SceneManager.GetActiveScene().name) {
            float x = PlayerPrefs.GetFloat("SpawnX");
            float y = PlayerPrefs.GetFloat("SpawnY");
            float z = PlayerPrefs.GetFloat("SpawnZ");

            transform.position = new Vector3(x, y, z);

            PlayerPrefs.DeleteKey("SpawnScene");

            Debug.Log("Spawn applied at: " + transform.position);
        }
    }
}