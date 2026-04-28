using System.Collections;

using UnityEngine;

public class PlayerLevelSpawn : MonoBehaviour {
    CharacterController cc;

    void Start() {
        cc = GetComponent<CharacterController>();
        StartCoroutine(ApplySpawn());
    }

    IEnumerator ApplySpawn() {
       
        yield return new WaitForEndOfFrame();

        string spawnID = SpawnData.spawnID;

        Debug.Log("Loaded SpawnID: " + spawnID);

        if (spawnID == "") {
            Debug.Log("No SpawnID found, using default position");
            yield break;
        }

        SpawnPos[] points = FindObjectsByType<SpawnPos>(FindObjectsSortMode.None);

        Debug.Log("Found spawn points: " + points.Length);

        foreach (SpawnPos point in points) {
            Debug.Log("Checking: " + point.spawnID);

            if (point.spawnID == spawnID) {
                if (cc != null) cc.enabled = false;

                transform.position = point.transform.position;
                transform.rotation = point.transform.rotation;

                if (cc != null) cc.enabled = true;

                Debug.Log("FINAL spawn applied at: " + spawnID);

                break;
            }
        }

        SpawnData.spawnID = "";
    }
}