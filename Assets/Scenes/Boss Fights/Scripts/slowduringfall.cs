using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class slowduringfall : MonoBehaviour {
    public float horizontalDamp = 0.3f; // lower = slower

    private CharacterController controller;
    private Vector3 lastPosition;

    void Start() {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void LateUpdate() {
        Vector3 movement = transform.position - lastPosition;

        // keep vertical movement, damp horizontal
        movement.x *= horizontalDamp;
        movement.z *= horizontalDamp;

        transform.position = lastPosition + movement;

        lastPosition = transform.position;
    }
}