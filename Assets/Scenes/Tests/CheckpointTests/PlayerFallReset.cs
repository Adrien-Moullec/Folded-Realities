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

        CharacterController cc = GetComponent<CharacterController>();
        Rigidbody rb = GetComponent<Rigidbody>();

        if (cc != null) cc.enabled = false;

        if (rb != null) {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        yield return new WaitForSeconds(0.05f);

        if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint()) {
            CheckpointManager.Instance.RespawnPlayer(gameObject);
        } else {
            if (startSpawnPoint != null) {

                Vector3 spawnPos = startSpawnPoint.position + Vector3.up * 2f;

                RaycastHit hit;
                if (Physics.Raycast(spawnPos, Vector3.down, out hit, 10f)) {
                    spawnPos.y = hit.point.y + 1f;
                }

                transform.position = spawnPos;
            }
        }

        yield return null;

        if (cc != null) cc.enabled = true;

        respawning = false;
    }
}