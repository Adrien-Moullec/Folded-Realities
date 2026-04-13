using UnityEngine;

public class PushableBlock : MonoBehaviour {
    private Rigidbody rb;
    private bool isBeingPushed = false;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (!isBeingPushed) {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        isBeingPushed = false;
    }

    public void SetPushed() {
        isBeingPushed = true;
    }
}