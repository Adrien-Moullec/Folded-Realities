using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {
    public string nextSceneName; 
    public Transform spawnPointInNextScene; 

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player"))
            return;

        // Save spawn position for next level
        PlayerPrefs.SetFloat("SpawnX", spawnPointInNextScene.position.x);
        PlayerPrefs.SetFloat("SpawnY", spawnPointInNextScene.position.y);
        PlayerPrefs.SetFloat("SpawnZ", spawnPointInNextScene.position.z);

        PlayerPrefs.SetString("SpawnScene", nextSceneName);
        PlayerPrefs.Save();

        SceneManager.LoadScene(nextSceneName);
    }
}