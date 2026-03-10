using UnityEngine;

public class PlayerDeath : MonoBehaviour {
    void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            Debug.Log("Player died - respawning");
            CheckpointManager.Instance.RespawnPlayer(gameObject);
        }
    }
}