using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySpawn : MonoBehaviour {
    [SerializeField] int doorID;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        string returnScene = PlayerPrefs.GetString("LastScene", "");

        if (string.IsNullOrEmpty(returnScene)) {
            Debug.Log("No previous level stored");
            return;
        }

        PlayerPrefs.SetInt("SpawnDoorID", doorID);
        PlayerPrefs.Save();

        SceneManager.LoadScene(returnScene);
    }
}