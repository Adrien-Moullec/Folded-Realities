using UnityEngine;

public class DebrisMotion : MonoBehaviour {
    [Header("Wiggle")]
    public float wiggleAmount = 0.5f;
    public float wiggleSpeed = 2f;

    [Header("Rotation")]
    public Vector3 rotationSpeed;

    private Vector3 startPos;
    private float randomOffset;

    void Start() {
        startPos = transform.position;

        // Randomise values per object
        wiggleAmount = Random.Range(0.2f, 1f);
        wiggleSpeed = Random.Range(1f, 4f);

        rotationSpeed = new Vector3(
            Random.Range(-50f, 50f),
            Random.Range(-50f, 50f),
            Random.Range(-50f, 50f)
        );

        randomOffset = Random.Range(0f, 100f);
    }

    void Update() {
        // Wiggle movement
        float x = Mathf.Sin(Time.time * wiggleSpeed + randomOffset) * wiggleAmount;
        float y = Mathf.Cos(Time.time * wiggleSpeed * 0.5f + randomOffset) * (wiggleAmount * 0.3f);

        transform.position = startPos + new Vector3(x, y, 0);

        // Rotation
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}