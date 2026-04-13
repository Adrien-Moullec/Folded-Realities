using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SlowFallOverride : MonoBehaviour {
    public float fallSpeed = 0.5f;

    [SerializeField] private MonoBehaviour playerController;
    [SerializeField] private Transform npc;
    public float minHeightAboveNPC = 2f;

    private CharacterController controller;
    private bool isFalling = true;

    void Start() {
        controller = GetComponent<CharacterController>();

        if (playerController != null) {
            playerController.enabled = false;
        }

        controller.enabled = false;
    }

    void Update() {
        if (!isFalling) {
            return;
        }

        Vector3 pos = transform.position;
        pos.y -= fallSpeed * Time.deltaTime;

        if (npc != null) {
            if (pos.y <= npc.position.y + minHeightAboveNPC) {
                pos.y = npc.position.y + minHeightAboveNPC;
            }
        }

        transform.position = pos;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f)) {
            Land();
        }
    }

    void Land() {
        isFalling = false;

        controller.enabled = true;

        if (playerController != null) {
            playerController.enabled = true;
        }

        this.enabled = false;
    }

    public void StopFalling() {
        Land();
    }
}