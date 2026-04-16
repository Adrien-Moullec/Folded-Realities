using UnityEngine;

public class DoorSpawnManager : MonoBehaviour {
    public Transform[] spawnPoints;

    void Start() {
        StartCoroutine(SetSpawn());
    }

    System.Collections.IEnumerator SetSpawn() {
        yield return null; // wait 1 frame

        int spawnID = PlayerPrefs.GetInt("SpawnPoint", 0);

        if (spawnID >= spawnPoints.Length) {
            spawnID = 0;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null) {
            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null) cc.enabled = false;

            player.transform.position = spawnPoints[spawnID].position;

            if (cc != null) cc.enabled = true;
        }
    }
}