using UnityEngine;

public class PushObjects : MonoBehaviour {
    public float pushSpeed = 3f;

    public float rigidbodyPushForce = 5f;

    void OnControllerColliderHit(
        ControllerColliderHit hit
    ) {
        Vector3 moveDir =
            new Vector3(
                hit.moveDirection.x,
                0,
                hit.moveDirection.z
            );

        if (
            moveDir.magnitude < 0.2f
        ) {
            return;
        }

        if (
            hit.collider.CompareTag(
                "Pushable"
            )
        ) {
            TOYCARPUSH car =
                hit.collider.GetComponent<
                    TOYCARPUSH
                >();

            if (car != null) {
                car.Push(
                    moveDir.normalized
                    * pushSpeed
                );

                return;
            }
        }

        Rigidbody rb =
            hit.collider.attachedRigidbody;

        if (rb == null) {
            return;
        }

        if (rb.isKinematic) {
            return;
        }

        rb.AddForce(
            moveDir.normalized
            * rigidbodyPushForce,
            ForceMode.Impulse
        );
    }
}