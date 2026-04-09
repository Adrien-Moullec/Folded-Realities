using UnityEngine;

public class FallingNPC : MonoBehaviour {
    public Transform player;
    public Transform ground; 

    [Header("Movement")]
    public float fallSpeed = 2f;
    public float slowFallSpeed = 0.8f;

    public float sideAmplitude = 2f;
    public float sideSpeed = 2f;

    public float verticalOffset = -5f;

    [Header("Catch")]
    public float slowDistanceFromGround = 4f;
    public float catchDistance = 2f;

    private float randomOffset;
    private bool caught = false;

    void Start() {
        randomOffset = Random.Range(0f, 100f);
    }

    void Update() {
        if (player == null || ground == null || caught) return;

        float groundY = ground.position.y;
        float distanceToGround = transform.position.y - groundY;

        float currentSpeed = fallSpeed;

        
        if (distanceToGround <= slowDistanceFromGround) {
            currentSpeed = slowFallSpeed;
            verticalOffset = -2f; // bring closer to player
        }

        float side = Mathf.Sin(Time.time * sideSpeed + randomOffset) * sideAmplitude;

        float targetY = player.position.y + verticalOffset;
        float newY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * currentSpeed);

       
        if (newY < groundY + 1f)
            newY = groundY + 1f;

        Vector3 newPos = new Vector3(
            player.position.x + side,
            newY,
            player.position.z
        );

        transform.position = newPos;

       
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToGround <= 2f && distToPlayer <= catchDistance) {
            CatchPlayer();
        }
    }

    void CatchPlayer() {
        caught = true;

        transform.SetParent(player);
        transform.localPosition = new Vector3(0.5f, 1f, 0.5f);

        Debug.Log("NPC CAUGHT!");
    }
}