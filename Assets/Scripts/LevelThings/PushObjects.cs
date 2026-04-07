using UnityEngine;

public class PushObjects : MonoBehaviour {

    public float pushForce = 5f;

    void OnControllerColliderHit(ControllerColliderHit hit) {

        if (!hit.collider.CompareTag("Pushable")) {
            return;
        }

        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb == null) {
            return;
        }

        Vector3 moveDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        if (moveDir.magnitude < 0.1f) {
            return;
        }

        rb.AddForce(moveDir * pushForce, ForceMode.Impulse);
    }
}