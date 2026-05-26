/*using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeleport : MonoBehaviour {
    [SerializeField] string sceneToLoad;
    [SerializeField] int spawnID;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        PlayersPrefs.SetInt("SpawnDoorID", spawnID);
        PlayersPrefs.SetInt("UseDoorSpawn", 1);
        PlayersPrefs.Save();

        SceneManager.LoadScene(sceneToLoad);
    }
}*/