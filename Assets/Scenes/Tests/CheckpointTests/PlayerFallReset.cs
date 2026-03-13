using UnityEngine;

public class PlayerFallReset : MonoBehaviour {
    public float fallYLimit = -10f;
    private bool respawning = false;

    void Update() {
        if (!respawning && transform.position.y < fallYLimit) {
            respawning = true;
            CheckpointManager.Instance.RespawnPlayer(gameObject);
            respawning = false;
        }
    }
}