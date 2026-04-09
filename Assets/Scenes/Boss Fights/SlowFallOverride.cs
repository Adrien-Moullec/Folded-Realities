using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SlowFallOverride : MonoBehaviour {
    public float fallSpeed = -2f; // gentle falling speed

    private CharacterController controller;
    private Vector3 lastPosition;

    void Start() {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void LateUpdate() {
        // Calculate how much the player moved this frame
        Vector3 movement = transform.position - lastPosition;

        // Replace the vertical movement with slow fall
        if (!controller.isGrounded) {
            movement.y = fallSpeed * Time.deltaTime;
        }

        // Apply corrected movement
        transform.position = lastPosition + movement;

        lastPosition = transform.position;
    }
}