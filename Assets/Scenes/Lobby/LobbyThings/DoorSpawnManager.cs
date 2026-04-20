using UnityEngine;

public class DoorSpawnManager : MonoBehaviour {
    [SerializeField] Transform[] doorSpawnPoints;
    [SerializeField] Transform defaultSpawnPoint;

    void Start() {
        StartCoroutine(SetSpawn());
    }

    System.Collections.IEnumerator SetSpawn() {
        yield return null; // wait 1 frame

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) yield break;

        CharacterController cc = player.GetComponent<CharacterController>();

        if (PlayerPrefs.GetInt("UseDoorSpawn", 0) == 1) {
            int doorID = PlayerPrefs.GetInt("SpawnDoorID", 0);

            if (doorID >= 0 && doorID < doorSpawnPoints.Length) {
                Transform spawn = doorSpawnPoints[doorID];

                if (cc != null) cc.enabled = false;

                // Slight upward offset ONLY (not forward)
                //Vector3 safePos = spawn.position + Vector3.up * 0.5f;
                //
                //player.transform.position = safePos;
                //player.transform.rotation = spawn.rotation;

                if (cc != null) cc.enabled = true;
            }

            PlayerPrefs.SetInt("UseDoorSpawn", 0);
        } else if (defaultSpawnPoint != null) {

            if (cc != null) cc.enabled = false;

            //player.transform.position = defaultSpawnPoint.position;
            //player.transform.rotation = defaultSpawnPoint.rotation;

            if (cc != null) cc.enabled = true;
        }
    }
}