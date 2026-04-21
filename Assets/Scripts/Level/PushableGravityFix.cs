using UnityEngine;

public class PushableGravityFix : MonoBehaviour {
    public float groundCheckDistance = 0.6f;

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        
        if (!Physics.Raycast(transform.position, Vector3.down, groundCheckDistance)) {
            
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y - 20f * Time.fixedDeltaTime, rb.linearVelocity.z);
        }
    }
}