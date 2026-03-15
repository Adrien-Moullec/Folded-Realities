using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    public static CheckpointManager Instance;

    public Transform levelStartSpawn; // drag your level start object here

    private Vector3 lastCheckpointPosition;
    private int currentCheckpointIndex = -1;

    private void Awake() {
        Instance = this;

        // initialise spawn position
        if (levelStartSpawn != null) {
            lastCheckpointPosition = levelStartSpawn.position;
        }
    }

    public void SetCheckpoint(Vector3 position, int checkpointIndex) {

        // Prevent activating old checkpoints
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

        Debug.Log("Respawning player");

        CharacterController cc = playerRoot.GetComponent<CharacterController>();

        if (cc != null) {
            cc.enabled = false;
            playerRoot.transform.position = lastCheckpointPosition;
            cc.enabled = true;
        } else {
            playerRoot.transform.position = lastCheckpointPosition;
        }
    }
}