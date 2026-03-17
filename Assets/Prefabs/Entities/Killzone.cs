using UnityEngine;

public class InstantDeathTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player"))
            return;

        if (CheckpointManager.Instance != null) {
            CheckpointManager.Instance.RespawnPlayer(other.gameObject);
        }
    }
}