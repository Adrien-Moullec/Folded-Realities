using UnityEngine;

public class PushObjects : MonoBehaviour {
    public float pushPower = 3f;

    void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        rb.linearVelocity = pushDir * pushPower;
    }
}