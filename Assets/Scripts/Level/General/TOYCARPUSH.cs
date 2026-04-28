using UnityEngine;

public class TOYCARPUSH : MonoBehaviour {

    public float moveSpeed = 3f;

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void Push(Vector3 inputMove) {
        Vector3 move = new Vector3(inputMove.x, 0, inputMove.z);
        rb.AddForce(move * moveSpeed, ForceMode.Force);
    }
}