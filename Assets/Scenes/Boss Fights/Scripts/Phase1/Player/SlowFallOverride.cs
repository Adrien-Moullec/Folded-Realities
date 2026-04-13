using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SlowFallOverride : MonoBehaviour {
    public float fallSpeed = 2f;

    private CharacterController controller;
    private bool canFall = true;

    void Start() {
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        if (!canFall) {
            return;
        }

        if (!controller.isGrounded) {
            controller.Move(Vector3.down * fallSpeed * Time.deltaTime);
        }
    }

    public void StopFalling() {
        canFall = false;
    }
}