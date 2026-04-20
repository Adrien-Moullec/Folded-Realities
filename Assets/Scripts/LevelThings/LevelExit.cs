using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

    public string nextSceneName = "EndOfLevel";

    private bool triggered = false;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || triggered) return;

        triggered = true;

       
        if (CollectiblesManager.Instance != null) {
            int coins = CollectiblesManager.Instance.GetCoinCount();
            PlayerPrefs.SetInt("FinalCoins", coins);
            Debug.Log("Saved coins: " + coins);
        }

        PlayerPrefs.SetInt("Stage2Unlocked", 1);
        PlayerPrefs.Save();

        
        SceneManager.LoadScene(nextSceneName);
    }
}