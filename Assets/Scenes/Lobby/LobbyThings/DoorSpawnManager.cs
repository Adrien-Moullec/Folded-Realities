using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorSpawnManager : MonoBehaviour {

    public Transform[] doorSpawnPoints;
    public Transform defaultSpawnPoint;

    void Start() {
        Debug.Log("DoorSpawnManager STARTED");

        // DEFAULT PLAY FIX (when not coming from a door)
        if (SceneManager.GetActiveScene().name == "Table_Area") {
            if (PlayerPrefs.GetInt("UseDoorSpawn", 0) != 1) {
                PlayerPrefs.SetInt("UseDoorSpawn", 0);
                Debug.Log("Default play detected - forcing default spawn");
            }
        }

        StartCoroutine(SetSpawn());
    }

    IEnumerator SetSpawn() {
        yield return null;

        Debug.Log("SPAWN CHECK START");

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) {
            Debug.LogError("Player NOT FOUND");
            yield break;
        } else {
            Debug.Log("Player found: " + player.name);
        }

        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc == null) {
            Debug.Log("No CharacterController found");
        }

        int useDoor = PlayerPrefs.GetInt("UseDoorSpawn", 0);
        Debug.Log("UseDoorSpawn value: " + useDoor);

        if (useDoor == 1) {

            float x = PlayerPrefs.GetFloat("SpawnX", -9999f);
            float y = PlayerPrefs.GetFloat("SpawnY", -9999f);
            float z = PlayerPrefs.GetFloat("SpawnZ", -9999f);

            Vector3 spawnPos = new Vector3(x, y, z);

            Debug.Log("Loaded spawn coords: " + spawnPos);

            if (x == -9999f || y == -9999f || z == -9999f) {
                Debug.LogError("Spawn coordinates not set");
            }

            if (cc != null) cc.enabled = false;

            player.transform.position = spawnPos + Vector3.up * 1f;

            Debug.Log("Player moved to: " + player.transform.position);

            if (cc != null) cc.enabled = true;

            PlayerPrefs.SetInt("UseDoorSpawn", 0);

        } else {

            Debug.Log("Using default spawn");

            if (defaultSpawnPoint == null) {
                Debug.LogError("Default spawn point not assigned");
                yield break;
            }

            Debug.Log("Default spawn position: " + defaultSpawnPoint.position);

            if (cc != null) cc.enabled = false;

            player.transform.position = defaultSpawnPoint.position + Vector3.up * 1f;

            Debug.Log("Player moved to default: " + player.transform.position);

            if (cc != null) cc.enabled = true;
        }

        Debug.Log("SPAWN CHECK END");
    }
}