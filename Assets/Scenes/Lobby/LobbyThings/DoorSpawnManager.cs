using UnityEngine;

public class DoorSpawnManager : MonoBehaviour {
    [SerializeField] Transform[] doorSpawnPoints;
    [SerializeField] float spawnOffset = 1.5f; // push player forward

    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            return;
        }

        int doorID = PlayerPrefs.GetInt("SpawnDoorID", 0);

        if (doorID >= 0 && doorID < doorSpawnPoints.Length) {
            Transform spawn = doorSpawnPoints[doorID];

            // push player OUT of  trigger
            Vector3 safePos = spawn.position + spawn.forward * spawnOffset;

            player.transform.position = safePos;
            player.transform.rotation = spawn.rotation;
        }
    }
}