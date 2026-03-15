using UnityEngine;

public class PlayerFallReset : MonoBehaviour {

    public float fallYLimit = -10f;

    public Transform startSpawnPoint; // assign in inspector

    private bool respawning = false;

    void Update() {

        if (!respawning && transform.position.y < fallYLimit) {

            respawning = true;

            // If a checkpoint exists use it
            if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint()) {
                CheckpointManager.Instance.RespawnPlayer(gameObject);
            } else {
                // otherwise go back to level start
                transform.position = startSpawnPoint.position;
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb) {
                    rb.linearVelocity = Vector3.zero;
                }
            }

            respawning = false;
        }
    }
}