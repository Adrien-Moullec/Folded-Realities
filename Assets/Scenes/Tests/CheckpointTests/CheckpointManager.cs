using UnityEngine;

using AbilitySystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour {

    public static CheckpointManager Instance;
    [SerializeField] PlayerAbilityController Player;
    [SerializeField] List<LevelExit> levelExits;
    public Transform levelStartSpawn;
    private Vector3 lastCheckpointPosition;
    private int currentCheckpointIndex = -1;
    private bool respawning = false;
    public float fallYLimit = -10f;

    private void Awake() {
        Instance = this;
        if (levelStartSpawn != null)
            lastCheckpointPosition = levelStartSpawn.position;
    }

    void Update() {
        if (!respawning && Player?.transform.position.y < fallYLimit) {
            StartCoroutine(GameplaySystem.instance.Respawn());
        }
    }
    public void SetCheckpoint(Vector3 position, int checkpointIndex, GameObject player = null) {

        if (checkpointIndex <= currentCheckpointIndex)
            return;

        currentCheckpointIndex = checkpointIndex;
        lastCheckpointPosition = position;

        if (player != null)
            if (player.TryGetComponent(out PlayerAbilityController abilityController))
                abilityController.SetMaxHealth();
    }

    public bool HasCheckpoint() => currentCheckpointIndex != -1;

    public Vector3 GetCheckpointPosition() {
        if (HasCheckpoint()) return lastCheckpointPosition;
        if (levelStartSpawn != null) return levelStartSpawn.position;
        return Vector3.zero;
    }

    public void RespawnPlayerIntoLevel(int spawnId = -1) {
        if (Player == null) return;
        if (spawnId == -1) {
            RespawnPlayer(false);
            return;
        }
        if (spawnId >= 0 && spawnId < levelExits.Count && levelExits[spawnId] != null) {
            SpawnPlayerAtLocation(levelExits[spawnId].SpawnPos + Vector3.up * 2f);
        } else {
            RespawnPlayer(false);
        }
    }
    void SpawnPlayer() {
        if (Player == null) return;
        Vector3 spawnPos = Vector3.zero;
        if (levelStartSpawn != null) {
            spawnPos = levelStartSpawn.position + Vector3.up * 2f;
            SpawnPlayerAtLocation(spawnPos);
        }
    }
    public void RespawnPlayer(bool savePoint = true) {
        if (Player == null) return;
        Vector3 spawnPos = Vector3.zero;
        if (levelStartSpawn != null) {
            spawnPos = levelStartSpawn.position + Vector3.up * 2f;
            spawnPos = savePoint ? GameplaySystem.GetSceneSavePoint(SceneManager.GetActiveScene().name, spawnPos) : spawnPos;
            SpawnPlayerAtLocation(spawnPos);
        }
    }
    void SpawnPlayerAtLocation(Vector3 pos) {
        if (Player == null) return;
        if (Player.TryGetComponent(out Rigidbody rb)) {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        if (Player.characterController != null) Player.characterController.enabled = false;

        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit, 10f))
            pos.y = hit.point.y + 1f;
        Player.transform.position = pos;

        if (Player.characterController != null) Player.characterController.enabled = true;
    }
}