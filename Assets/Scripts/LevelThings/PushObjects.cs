using UnityEngine;

public class PushObjects : MonoBehaviour {

    public float pushSpeed = 3f;

    void OnControllerColliderHit(ControllerColliderHit hit) {

        if (!hit.collider.CompareTag("Pushable")) {
            return;
        }

        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb == null) {
            return;
        }

        
        Vector3 moveDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z).normalized;

        
        Vector3 cubeForward = hit.collider.transform.forward;

        
        float dot = Vector3.Dot(moveDir, cubeForward);

       
        if (Mathf.Abs(dot) < 0.8f) {
            return;
        }

       
        Vector3 pushDir = cubeForward * Mathf.Sign(dot);

        
        Vector3 move = pushDir * pushSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);

       
        PushableBlock block = hit.collider.GetComponent<PushableBlock>();
        if (block != null) {
            block.SetPushed();
        }
    }
}