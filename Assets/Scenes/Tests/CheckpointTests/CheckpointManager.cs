using UnityEngine;

using AbilitySystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour {

    public static CheckpointManager Instance;
    [SerializeField] PlayerAbilityController Player;
    // List of level exits used for spawn locations
    [SerializeField] List<LevelExit> levelExits;
    // Default level start spawn point
    public Transform levelStartSpawn;
    private Vector3 lastCheckpointPosition;
    private int currentCheckpointIndex = -1;
    private bool respawning = false;
    // Y position that triggers fall death
    public float fallYLimit = -10f;

    private void Awake() {
        // Sets singleton instance
        Instance = this;
        if (levelStartSpawn != null)
            lastCheckpointPosition = levelStartSpawn.position;
    }
    #region Respawn Player & Checks for checkpoints
    void Update() {
        // Respawns player if they fall below limit
        if (!respawning && Player?.transform.position.y < fallYLimit) {
            StartCoroutine(GameplaySystem.instance.Respawn());
        }
    }
    public void SetCheckpoint(Vector3 position, int checkpointIndex, GameObject player = null) {

        if (checkpointIndex <= currentCheckpointIndex)
            return;
        // Prevents older checkpoints replacing newer ones
        currentCheckpointIndex = checkpointIndex;
        lastCheckpointPosition = position;
        // Restores player health at checkpoint
        if (player != null)
            if (player.TryGetComponent(out PlayerAbilityController abilityController))
                abilityController.SetMaxHealth();
    }

    public bool HasCheckpoint() => currentCheckpointIndex != -1;
    // Returns latest checkpoint position
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
        // Respawns using specific level exit position
        if (spawnId >= 0 && spawnId < levelExits.Count && levelExits[spawnId] != null) {
            SpawnPlayerAtLocation(levelExits[spawnId].SpawnPos + Vector3.up * 2f);
        } else {
            RespawnPlayer(false);
        }
    }
    #endregion

    #region Spawn Player @ Location
    void SpawnPlayer() {
        // Prevents errors if player missing
        if (Player == null) return;
        Vector3 spawnPos = Vector3.zero;
        if (levelStartSpawn != null) {
            spawnPos = levelStartSpawn.position + Vector3.up * 2f;
            SpawnPlayerAtLocation(spawnPos);
        }
    }  // Loads saved checkpoint position
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

        // Disables character controller before moving
        if (Player.characterController != null) Player.characterController.enabled = false;

        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit, 10f))
            pos.y = hit.point.y + 1f;
        Player.transform.position = pos;

        if (Player.characterController != null) Player.characterController.enabled = true;
    }
}
#endregion