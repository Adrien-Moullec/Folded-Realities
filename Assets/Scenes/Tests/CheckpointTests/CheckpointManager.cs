using UnityEngine;

public class CheckpointManager : MonoBehaviour {
    public static CheckpointManager Instance;

    private Vector3 lastCheckpointPosition;
    private int currentCheckpointIndex = -1;

    private void Awake() {
        Instance = this;
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

    public void RespawnPlayer(GameObject playerRoot) {
        if (currentCheckpointIndex == -1) {
            Debug.Log("No checkpoint set!");
            return;
        }

        Debug.Log("Respawning player at checkpoint");

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