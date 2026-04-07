using UnityEngine;

public class DoorSpawnManager : MonoBehaviour {
    [SerializeField] Transform[] doorSpawnPoints;
    [SerializeField] Transform defaultSpawnPoint;
    [SerializeField] float spawnOffset = 1.5f;

    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            return;
        }

        
        if (PlayerPrefs.GetInt("UseDoorSpawn", 0) == 1) {
            int doorID = PlayerPrefs.GetInt("SpawnDoorID", 0);

            if (doorID >= 0 && doorID < doorSpawnPoints.Length) {
                Transform spawn = doorSpawnPoints[doorID];

                Vector3 safePos = spawn.position + spawn.forward * spawnOffset;

                player.transform.position = safePos;
                player.transform.rotation = spawn.rotation;
            }

            
            PlayerPrefs.SetInt("UseDoorSpawn", 0);
        } else if (defaultSpawnPoint != null) {
            // normal play mode start
            player.transform.position = defaultSpawnPoint.position;
            player.transform.rotation = defaultSpawnPoint.rotation;
        }
    }
}