using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour {

    [Header("Scene to Restart")]
    public string levelName = "The Bin";

    public void RestartLevel() {


        //PlayerPrefs.DeleteKey("FinalCoins");


        if (CollectiblesManager.Instance != null) {
            CollectiblesManager.Instance.normalCount = 0;
        }

        Debug.Log("Restarting level: " + levelName);


        SceneManager.LoadScene(levelName);
    }
}