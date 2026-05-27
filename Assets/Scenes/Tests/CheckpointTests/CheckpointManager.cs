using UnityEngine;
using AbilitySystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour {

    public static CheckpointManager Instance;

    [SerializeField]
    PlayerAbilityController Player;

    [Header("Level Exits")]
    [SerializeField]
    List<LevelExit> levelExits;

    [Header("Spawn")]
    public Transform levelStartSpawn;

    [Header("Fall Death")]
    public float fallYLimit = -10f;

    Vector3 lastCheckpointPosition;

    int currentCheckpointIndex = -1;

    bool respawning = false;

    void Awake() {

        Instance = this;

        if (Player == null) {
            Player =
                FindFirstObjectByType<PlayerAbilityController>();
        }

        if (levelStartSpawn != null) {
            lastCheckpointPosition =
                levelStartSpawn.position;
        }
    }

    void Update() {

        if (Player == null) {

            Player =
                FindFirstObjectByType<PlayerAbilityController>();

            return;
        }

        if (
            !respawning &&
            Player.transform.position.y < fallYLimit
        ) {

            StartCoroutine(
                RespawnRoutine()
            );
        }
    }

    IEnumerator RespawnRoutine() {

        respawning = true;

        yield return GameplaySystem.instance.Respawn();

        respawning = false;
    }

    public void SetCheckpoint(
        Vector3 position,
        int checkpointIndex,
        GameObject player = null
    ) {

        if (
            checkpointIndex <=
            currentCheckpointIndex
        ) {
            return;
        }

        currentCheckpointIndex =
            checkpointIndex;

        lastCheckpointPosition =
            position;

        if (player != null) {

            IHealth ihealth =
                player.GetComponentInChildren<IHealth>();

            if (ihealth != null) {

                ihealth.Heal(
                    new EntityDamage(
                        9999,
                        null
                    )
                );

                Debug.Log(
                    "Player health restored"
                );
            } else {

                Debug.LogWarning(
                    "No IHealth found on player"
                );
            }
        }
    }

    public bool HasCheckpoint() {

        return currentCheckpointIndex != -1;
    }

    public Vector3 GetCheckpointPosition() {

        if (HasCheckpoint()) {
            return lastCheckpointPosition;
        }

        if (levelStartSpawn != null) {
            return levelStartSpawn.position;
        }

        return Vector3.zero;
    }

    public void RespawnPlayerIntoLevel(
        int spawnId = -1
    ) {

        if (Player == null) {
            return;
        }

        if (spawnId == -1) {

            RespawnPlayer(false);

            return;
        }

        if (
            spawnId >= 0 &&
            spawnId < levelExits.Count &&
            levelExits[spawnId] != null
        ) {

            SpawnPlayerAtLocation(
                levelExits[spawnId].SpawnPos +
                Vector3.up * 2f
            );
        } else {

            RespawnPlayer(false);
        }
    }

    void SpawnPlayer() {

        if (Player == null) {
            return;
        }

        Vector3 spawnPos = Vector3.zero;

        if (levelStartSpawn != null) {

            spawnPos =
                levelStartSpawn.position +
                Vector3.up * 2f;

            SpawnPlayerAtLocation(
                spawnPos
            );
        }
    }

    public void RespawnPlayer(
        bool savePoint = true
    ) {

        if (Player == null) {
            return;
        }

        Vector3 spawnPos = Vector3.zero;

        if (levelStartSpawn != null) {

            spawnPos =
                levelStartSpawn.position +
                Vector3.up * 2f;

            spawnPos =
                savePoint
                ? GameplaySystem.GetSceneSavePoint(
                    SceneManager.GetActiveScene().name,
                    spawnPos
                )
                : spawnPos;

            SpawnPlayerAtLocation(
                spawnPos
            );
        }
    }

    void SpawnPlayerAtLocation(
        Vector3 pos
    ) {

        if (Player == null) {
            return;
        }

        if (
            Player.TryGetComponent(
                out Rigidbody rb
            )
        ) {

            rb.linearVelocity =
                Vector3.zero;

            rb.angularVelocity =
                Vector3.zero;
        }

        if (
            Player.characterController != null
        ) {

            Player.characterController.enabled =
                false;
        }

        RaycastHit hit;

        if (
            Physics.Raycast(
                pos,
                Vector3.down,
                out hit,
                10f
            )
        ) {

            pos.y =
                hit.point.y + 1f;
        }

        Player.transform.position =
            pos;

        if (
            Player.characterController != null
        ) {

            Player.characterController.enabled =
                true;
        }
    }
}