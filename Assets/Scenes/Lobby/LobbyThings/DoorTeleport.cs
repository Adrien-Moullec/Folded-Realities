using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeleport : MonoBehaviour {
    [SerializeField] string sceneToLoad;
    [SerializeField] int spawnID; // where player will appear 

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        PlayerPrefs.SetInt("SpawnPoint", spawnID);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneToLoad);
    }
}