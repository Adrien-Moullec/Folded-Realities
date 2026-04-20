using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeleport : MonoBehaviour {
    [SerializeField] string sceneToLoad;
    [SerializeField] int spawnID;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        PlayerPrefs.SetInt("SpawnDoorID", spawnID);
        PlayerPrefs.SetInt("UseDoorSpawn", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneToLoad);
    }
}