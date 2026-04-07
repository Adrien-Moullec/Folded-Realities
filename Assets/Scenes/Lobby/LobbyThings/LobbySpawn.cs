using UnityEngine;

public class LobbySpawn : MonoBehaviour {
    [SerializeField] Transform[] spawnPoints;

    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        int spawnIndex = PlayerPrefs.GetInt("LobbySpawnPoint", 0);

        if (spawnIndex >= 0 && spawnIndex < spawnPoints.Length) {
            player.transform.position = spawnPoints[spawnIndex].position;
            player.transform.rotation = spawnPoints[spawnIndex].rotation;
        } else {
            Debug.LogWarning("Invalid spawn index, defaulting to 0");
            player.transform.position = spawnPoints[0].position;
        }
    }
}