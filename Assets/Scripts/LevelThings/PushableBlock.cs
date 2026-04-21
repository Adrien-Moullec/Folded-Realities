using UnityEngine;

public class PushableBlock : MonoBehaviour {

    private Rigidbody rb;
    private bool isBeingPushed = false;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {

        if (!isBeingPushed) {
            Vector3 vel = rb.linearVelocity;
            vel.x = 0f;
            vel.z = 0f;
            rb.linearVelocity = vel;
        }

        isBeingPushed = false;
    }

    public void SetPushed() {
        isBeingPushed = true;
    }
}