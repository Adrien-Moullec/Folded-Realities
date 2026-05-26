/*using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLobby : MonoBehaviour {
    [SerializeField] string lobbySceneName = "Lobby";
    [SerializeField] int doorID; 

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        PlayersPrefs.SetInt("LobbySpawnPoint", doorID);
        GameplaySystem.SaveSettings();

        SceneManager.LoadScene(lobbySceneName);
    }
}*/