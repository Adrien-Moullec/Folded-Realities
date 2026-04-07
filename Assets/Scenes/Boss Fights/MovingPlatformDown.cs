using UnityEngine;

public class MovingPlatformDown : MonoBehaviour {
    [Header("Movement")]
    public float fallSpeed = 3f;

    [Header("Respawn")]
    public float resetHeight = 15f;
    public float despawnOffset = 10f;
    public float horizontalRange = 4f;

    private Transform player;
    private CharacterController playerController;

    private Vector3 lastPosition;

    void Start() {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null) {
            player = playerObj.transform;
            playerController = playerObj.GetComponent<CharacterController>();
        }

        lastPosition = transform.position;
    }

    void Update() {
        if (player == null) {
            return;
        }

        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        Vector3 platformDelta = transform.position - lastPosition;

       
        if (IsPlayerOnPlatform()) {
            if (playerController != null) {
                playerController.Move(platformDelta);
            } else {
                player.position += platformDelta;
            }
        }

        lastPosition = transform.position;

       
        if (transform.position.y < player.position.y - despawnOffset) {
            ResetPlatform();
        }
    }

    bool IsPlayerOnPlatform() {
       
        float distance = player.position.y - transform.position.y;

        return distance > 0f && distance < 2f;
    }

    void ResetPlatform() {
        float randomX = Random.Range(-horizontalRange, horizontalRange);

        transform.position = new Vector3(
            randomX,
            player.position.y + resetHeight,
            transform.position.z
        );

        lastPosition = transform.position;
    }
}