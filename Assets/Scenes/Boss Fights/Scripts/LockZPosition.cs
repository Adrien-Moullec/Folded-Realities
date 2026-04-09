using UnityEngine;

public class LockZPosition : MonoBehaviour {
    public float lockedZ = 0f; 

    void LateUpdate() {
        Vector3 pos = transform.position;
        pos.z = lockedZ;
        transform.position = pos;
    }
}