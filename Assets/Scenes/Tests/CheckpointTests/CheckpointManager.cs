using UnityEngine;

using AbilitySystem;

public class CheckpointManager : MonoBehaviour {

    public static CheckpointManager Instance;

    public Transform levelStartSpawn;

    private Vector3 lastCheckpointPosition;

    private int currentCheckpointIndex = -1;

    private void Awake() {

        Instance = this;

        if (
            levelStartSpawn != null
        ) {
            lastCheckpointPosition =
                levelStartSpawn.position;
        }
    }

    public void SetCheckpoint(
        Vector3 position,
        int checkpointIndex,
        GameObject player = null
    ) {

        if (
            checkpointIndex
            <= currentCheckpointIndex
        ) {
            return;
        }

        currentCheckpointIndex =
            checkpointIndex;

        lastCheckpointPosition =
            position;

        if (
            player != null
        ) {

            PlayerAbilityController abilityController =
                player.GetComponent<
                    PlayerAbilityController
                >();

            if (
                abilityController != null
            ) {
                abilityController.SetMaxHealth();
            }
        }
    }

    public bool HasCheckpoint() {

        return
            currentCheckpointIndex != -1;
    }

    public Vector3 GetCheckpointPosition() {

        if (
            HasCheckpoint()
        ) {
            return
                lastCheckpointPosition;
        }

        if (
            levelStartSpawn != null
        ) {
            return
                levelStartSpawn.position;
        }

        return
            Vector3.zero;
    }

    public void RespawnPlayer(
        GameObject playerRoot
    ) {

        if (
            PlayerPrefs.GetInt(
                "UseDoorSpawn",
                0
            ) == 1
        ) {
            return;
        }

        CharacterController cc =
            playerRoot.GetComponent<
                CharacterController
            >();

        if (
            cc != null
        ) {
            cc.enabled = false;
        }

        Vector3 spawnPos;

        if (
            HasCheckpoint()
        ) {

            spawnPos =
                lastCheckpointPosition;
        } else if (
              levelStartSpawn != null
          ) {

            spawnPos =
                levelStartSpawn.position;
        } else {
            return;
        }

        spawnPos +=
            Vector3.up * 1f;

        playerRoot.transform.position =
            spawnPos;

        if (
            cc != null
        ) {
            cc.enabled = true;
        }
    }
}