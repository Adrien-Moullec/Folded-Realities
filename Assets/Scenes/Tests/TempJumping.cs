using UnityEngine;

public class TempJumpFix : MonoBehaviour {
    public CharacterController controller;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    private bool isGrounded;

    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetButtonDown("Jump")) {
            Debug.Log(isGrounded ? "Jump allowed" : "Jump blocked");
        }
    }


    void OnDrawGizmos() {
        if (groundCheck == null) {
            return;
        }

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}