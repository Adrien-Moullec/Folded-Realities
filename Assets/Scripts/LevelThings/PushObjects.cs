using UnityEngine;

public class PushObjects : MonoBehaviour {
    public float pushSpeed = 3f;

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (!hit.collider.CompareTag("Pushable")) {
            return;
        }

        Vector3 moveDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        if (moveDir.magnitude < 0.5f) {
            return;
        }

        Transform block = hit.collider.transform;

        block.position += moveDir.normalized * pushSpeed * Time.deltaTime;
    }
}