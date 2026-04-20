using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ContinueGame : MonoBehaviour {
    public string lobbySceneName = "Lobby";
    public int spawnID = 0;

    public bool autoContinue = true;
    public float delay = 2f;

    void Start() {
        if (autoContinue) {
            StartCoroutine(AutoLoad());
        }
    }

    IEnumerator AutoLoad() {
        yield return new WaitForSeconds(delay);
        LoadLobby();
    }

    public void LoadLobby() {
        
        PlayerPrefs.SetInt("SpawnID", spawnID);
        PlayerPrefs.Save();

        Debug.Log("Loading Lobby with SpawnID: " + spawnID);

        SceneManager.LoadScene(lobbySceneName);
    }
}