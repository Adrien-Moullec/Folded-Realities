using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

    public string nextSceneName = "EndOfLevel";

    public Vector3 nextSpawnPosition;
    public Vector3 nextSpawnRotation;

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

        
        PlayerPrefs.SetInt("UseDoorSpawn", 1);

        PlayerPrefs.SetFloat("SpawnX", nextSpawnPosition.x);
        PlayerPrefs.SetFloat("SpawnY", nextSpawnPosition.y);
        PlayerPrefs.SetFloat("SpawnZ", nextSpawnPosition.z);

        PlayerPrefs.SetFloat("RotX", nextSpawnRotation.x);
        PlayerPrefs.SetFloat("RotY", nextSpawnRotation.y);
        PlayerPrefs.SetFloat("RotZ", nextSpawnRotation.z);

        PlayerPrefs.Save();

        SceneManager.LoadScene(nextSceneName);
    }
}