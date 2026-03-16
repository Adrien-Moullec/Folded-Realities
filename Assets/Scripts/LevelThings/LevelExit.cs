using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {
    public string nextSceneName;

    [SerializeField] private Vector3 spawnPositionInNextScene;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger entered by: " + other.name);

        if (!other.CompareTag("Player")) {
            Debug.Log("Object entered trigger but is NOT the player.");
            return;
        }

        Debug.Log("Player entered level exit trigger.");

        // Save spawn position
        PlayerPrefs.SetFloat("SpawnX", spawnPositionInNextScene.x);
        PlayerPrefs.SetFloat("SpawnY", spawnPositionInNextScene.y);
        PlayerPrefs.SetFloat("SpawnZ", spawnPositionInNextScene.z);

        PlayerPrefs.SetString("SpawnScene", nextSceneName);
        PlayerPrefs.Save();

        Debug.Log("Spawn position saved: " + spawnPositionInNextScene);
        Debug.Log("Loading scene: " + nextSceneName);

        SceneManager.LoadScene(nextSceneName);
    }
}