using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeleport : MonoBehaviour {
    [SerializeField] string sceneToLoad;
    [SerializeField] int doorID;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        PlayerPrefs.SetInt("SpawnDoorID", doorID);
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);

        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneToLoad);
    }
}