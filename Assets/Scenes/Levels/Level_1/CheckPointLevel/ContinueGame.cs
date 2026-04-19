using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueGame : MonoBehaviour {

    [SerializeField] string lobbySceneName = "Lobby";
    [SerializeField] int spawnID = 0;

    public void Continue() {

        Debug.Log("Continue pressed");

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerPrefs.SetInt("SpawnDoorID", spawnID);
        PlayerPrefs.Save();

        SceneManager.LoadScene(lobbySceneName);
    }
}