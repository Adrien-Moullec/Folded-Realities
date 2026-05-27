using UnityEngine;

public class PushObjects : MonoBehaviour {

    // Speed used for custom push objects
    public float pushSpeed = 3f;

    // Force applied to rigidbodies
    public float rigidbodyPushForce = 5f;

    void OnControllerColliderHit(ControllerColliderHit hit) {

        // Gets horizontal push direction
        Vector3 moveDir = new Vector3(
            hit.moveDirection.x,
            0,
            hit.moveDirection.z
        );

        // Ignores tiny movement collisions
        if (moveDir.magnitude < 0.2f)
            return;

        // Handles custom pushable objects
        if (hit.collider.CompareTag("Pushable")) {

            TOYCARPUSH car = hit.collider.GetComponent<TOYCARPUSH>();

            if (car != null) {

                car.Push(moveDir.normalized * pushSpeed);

                return;
            }
        }

        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic)
            return;

        // Pushes rigidbody objects
        rb.AddForce(
            moveDir.normalized * rigidbodyPushForce,
            ForceMode.Impulse
        );
    }
}