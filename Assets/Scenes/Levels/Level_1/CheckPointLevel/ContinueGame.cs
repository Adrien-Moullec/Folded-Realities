using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueGame : MonoBehaviour {

    public string nextSceneName = "Lobby";

    public void Continue() {
        Debug.Log("Button Pressed - Loading Scene");
        SceneManager.LoadScene(nextSceneName);
    }
}