using UnityEngine;

public class KillZone : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player"))
            {
            return;
        }

        if (CheckpointManager.Instance != null) {
            CheckpointManager.Instance.RespawnPlayer(other.gameObject);
        }
    }
}