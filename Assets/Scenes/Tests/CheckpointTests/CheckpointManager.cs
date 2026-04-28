using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    public static CheckpointManager Instance;

    public Transform levelStartSpawn;

    private Vector3 lastCheckpointPosition;
    private int currentCheckpointIndex = -1;

    private void Awake() {
        Instance = this;

        if (levelStartSpawn != null) {
            lastCheckpointPosition = levelStartSpawn.position;
        } else {
            Debug.LogError("Level start spawn not assigned!");
        }
    }

    public void SetCheckpoint(Vector3 position, int checkpointIndex) {

        if (checkpointIndex <= currentCheckpointIndex) {
            return;
        }

        currentCheckpointIndex = checkpointIndex;
        lastCheckpointPosition = position;

        Debug.Log("Checkpoint reached: " + checkpointIndex);
    }

    public bool HasCheckpoint() {
        return currentCheckpointIndex != -1;
    }

    public void RespawnPlayer(GameObject playerRoot) {

        
        if (PlayerPrefs.GetInt("UseDoorSpawn", 0) == 1) {
            Debug.Log("Skipping checkpoint spawn (door spawn active)");
            return;
        }

        Debug.Log("Respawning player");

        CharacterController cc = playerRoot.GetComponent<CharacterController>();

        if (cc != null) cc.enabled = false;

        Vector3 spawnPos;

        if (HasCheckpoint()) {
            spawnPos = lastCheckpointPosition;
            Debug.Log("Using checkpoint position: " + spawnPos);
        } else if (levelStartSpawn != null) {
            spawnPos = levelStartSpawn.position;
            Debug.Log("Using level start position: " + spawnPos);
        } else {
            spawnPos = Vector3.zero;
            Debug.LogError("No spawn point assigned");
        }

        spawnPos += Vector3.up * 1f;

        playerRoot.transform.position = spawnPos;

        if (cc != null) cc.enabled = true;
    }
}