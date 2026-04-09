using UnityEngine;

public class LockYPosition : MonoBehaviour {
    private float lockedY;

    void Start() {
        lockedY = transform.position.y;
    }

    void LateUpdate() {
        Vector3 pos = transform.position;
        pos.y = lockedY;
        transform.position = pos;
    }
}