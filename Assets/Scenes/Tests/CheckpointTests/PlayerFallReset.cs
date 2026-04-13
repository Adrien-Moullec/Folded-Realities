using UnityEngine;
using System.Collections;

public class PlayerFallReset : MonoBehaviour {

    public float fallYLimit = -10f;
    public Transform startSpawnPoint;

    private bool respawning = false;

    void Update() {

        if (!respawning && transform.position.y < fallYLimit) {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn() {
        respawning = true;

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null) {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

       
        yield return new WaitForSeconds(0.1f);

        if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint()) {
            CheckpointManager.Instance.RespawnPlayer(gameObject);
        } else {
            transform.position = startSpawnPoint.position;
        }

        yield return new WaitForSeconds(0.2f);

        respawning = false;
    }
}