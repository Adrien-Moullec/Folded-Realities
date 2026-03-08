using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        hasCheckpoint = true;
    }

    public void RespawnPlayer(GameObject playerRoot)
    {
        if (!hasCheckpoint) return;

        playerRoot.transform.position = lastCheckpointPosition;
    }
}