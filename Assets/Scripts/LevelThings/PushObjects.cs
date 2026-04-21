using UnityEngine;

public class PushObjects : MonoBehaviour {

    public float pushSpeed = 3f;

    void OnControllerColliderHit(ControllerColliderHit hit) {

        if (!hit.collider.CompareTag("Pushable")) return;

        TOYCARPUSH car = hit.collider.GetComponent<TOYCARPUSH>();
        if (car == null) return;

        Vector3 moveDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        if (moveDir.magnitude < 0.2f) return;

        car.Push(moveDir.normalized * pushSpeed);
    }
}