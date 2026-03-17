using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TestJumpFix : MonoBehaviour {
    public float gravity = -20f;
    public float stickToGroundForce = -2f;

    private CharacterController cc;
    private float verticalVelocity;

    void Start() {
        cc = GetComponent<CharacterController>();
    }

    void Update() {
        // If grounded, keep player stuck to ground
        if (cc.isGrounded) {
            if (verticalVelocity < 0) {
                verticalVelocity = stickToGroundForce;
            }
        } else {
            // Apply gravity
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Apply vertical movement ONLY (doesn't break your system)
        cc.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}