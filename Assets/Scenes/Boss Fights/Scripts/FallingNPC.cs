using UnityEngine;

public class FallingNPC : MonoBehaviour {
    public Transform player;

    [Header("Movement")]
    public float fallSpeed = 2f;
    public float sideSpeed = 2f;
    public float sideAmplitude = 2f;
    public float verticalOffset = -5f;

    [Header("Avoidance")]
    public float avoidanceRadius = 3f;
    public float avoidanceStrength = 2f;
    public LayerMask debrisLayer;

    private float randomOffset;

    void Start() {
        randomOffset = Random.Range(0f, 100f);
    }

    void Update() {
        if (player == null) return;

        // Base side movement
        float side = Mathf.Sin(Time.time * sideSpeed + randomOffset) * sideAmplitude;

        float targetY = player.position.y + verticalOffset;
        float newY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * fallSpeed);

        Vector3 pos = new Vector3(
            player.position.x + side,
            newY,
            player.position.z
        );

       
        Collider[] nearby = Physics.OverlapSphere(transform.position, avoidanceRadius, debrisLayer);

        foreach (Collider col in nearby) {
            Vector3 dir = transform.position - col.transform.position;
            pos.x += dir.normalized.x * avoidanceStrength;
        }

        transform.position = pos;
    }
}