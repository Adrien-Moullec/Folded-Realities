using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueGame : MonoBehaviour {

    public string nextSceneName = "Lobby";

    public void Continue() {
        SceneManager.LoadScene(nextSceneName);
    }
}