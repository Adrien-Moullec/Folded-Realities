using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

    [SerializeField] Transform[] spawnPoints;

    void Start() {
        StartCoroutine(SetSpawn());
    }

    System.Collections.IEnumerator SetSpawn() {
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) yield break;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        int spawnID = PlayerPrefs.GetInt("SpawnDoorID", -1);

        if (spawnID >= 0 && spawnID < spawnPoints.Length) {
            Transform spawn = spawnPoints[spawnID];

            player.transform.position = spawn.position + Vector3.up * 1f;
            player.transform.rotation = spawn.rotation;

            Debug.Log("Spawned at spawn ID: " + spawnID);
        }

        if (cc != null) cc.enabled = true;
    }
}