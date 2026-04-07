using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLobby : MonoBehaviour {
    [SerializeField] string lobbySceneName = "Lobby";
    [SerializeField] int doorID; 

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        PlayerPrefs.SetInt("LobbySpawnPoint", doorID);
        PlayerPrefs.Save();

        SceneManager.LoadScene(lobbySceneName);
    }
}