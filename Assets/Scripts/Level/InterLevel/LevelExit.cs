using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {
    public string nextSceneName = "Bedroom";
    public string targetSpawnID = "1";

    private bool triggered = false;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || triggered) return;

        triggered = true;

        Debug.Log("SETTING SpawnID: " + targetSpawnID);

        SpawnData.spawnID = targetSpawnID;

        SceneManager.LoadScene(nextSceneName);
    }
}