using UnityEngine;

public class LockZPosition : MonoBehaviour {
    [Header("Z Lock")]
    public float lockedZ = 0f;

    [Header("Lane Reference")]
    public Transform spawner; // drag your cube/spawner here

    private float minX;
    private float maxX;

    void Start() {
        if (spawner != null) {
            float halfWidth = spawner.localScale.x * 0.5f;

            float centerX = spawner.position.x;

            minX = centerX - halfWidth + 1f;
            maxX = centerX + halfWidth - 1f;
        }
    }

    void LateUpdate() {
        Vector3 pos = transform.position;

        pos.z = lockedZ;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        transform.position = pos;
    }
}