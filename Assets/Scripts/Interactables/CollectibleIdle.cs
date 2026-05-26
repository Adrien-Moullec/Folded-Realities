/*using UnityEngine;

public class CollectibleIdle : MonoBehaviour
{
    public float hoverHeight = 0.25f;
    public float hoverSpeed = 2f;
    public float rotationSpeed = 90f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
*/