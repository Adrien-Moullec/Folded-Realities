using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

    [Header("Scene Settings")]
    public string nextSceneName;

    [Header("Optional Spawn (leave OFF for UI scenes)")]
    public bool useSpawnPoint = false;
    [SerializeField] private Vector3 spawnPositionInNextScene;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger entered by: " + other.name);

        if (!other.CompareTag("Player")) {
            Debug.Log("Not player, ignoring.");
            return;
        }

        Debug.Log("Player entered level exit.");


        if (CollectiblesManager.Instance != null) {
            int coins = CollectiblesManager.Instance.GetCoinCount();
            PlayerPrefs.SetInt("FinalCoins", coins);
            Debug.Log("Saved coins: " + coins);
        } else {
            Debug.LogWarning("CollectiblesManager not found!");
        }


        if (useSpawnPoint) {
            PlayerPrefs.SetFloat("SpawnX", spawnPositionInNextScene.x);
            PlayerPrefs.SetFloat("SpawnY", spawnPositionInNextScene.y);
            PlayerPrefs.SetFloat("SpawnZ", spawnPositionInNextScene.z);

            PlayerPrefs.SetString("SpawnScene", nextSceneName);
            PlayerPrefs.Save();

            Debug.Log("Spawn saved: " + spawnPositionInNextScene);
        }

        Debug.Log("Loading scene: " + nextSceneName);
        SceneManager.LoadScene(1);
    }
}