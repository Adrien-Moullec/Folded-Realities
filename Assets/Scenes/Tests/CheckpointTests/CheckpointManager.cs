using UnityEngine;

public class CheckpointManager : MonoBehaviour {
    public static CheckpointManager Instance;

    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;

    private void Awake() {
        Instance = this;
    }

    public void SetCheckpoint(Vector3 position) {
        lastCheckpointPosition = position;
        hasCheckpoint = true;

        Debug.Log("Checkpoint reached at: " + position);
    }

    public void RespawnPlayer(GameObject playerRoot) {
        if (!hasCheckpoint) {
            Debug.Log("No checkpoint set!");
            return;
        }

        Debug.Log("Respawning player at checkpoint");

        playerRoot.transform.position = lastCheckpointPosition;
    }
}