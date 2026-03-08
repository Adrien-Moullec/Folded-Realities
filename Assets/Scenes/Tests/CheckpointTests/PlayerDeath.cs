using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // Press K to simulate death
        {
            CheckpointManager.Instance.RespawnPlayer(gameObject);
        }
    }
}