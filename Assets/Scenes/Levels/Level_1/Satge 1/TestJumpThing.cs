using UnityEngine;

public class TestJumpThing : MonoBehaviour {
    public Transform feetTransform;
    public float radius = 0.25f;
    public LayerMask groundLayers;

    private CharacterController controller;

    void Start() {
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        if (feetTransform == null) {
            Debug.LogWarning("Feet Transform NOT assigned!");
            return;
        }

        Vector3 feetPos = feetTransform.position;

        bool sphereGrounded = Physics.CheckSphere(feetPos, radius, groundLayers);
        bool controllerGrounded = controller != null && controller.isGrounded;

        Collider[] hits = Physics.OverlapSphere(feetPos, radius);

        Debug.Log("----- GROUND DEBUG -----");
        Debug.Log("SphereGrounded: " + sphereGrounded);
        Debug.Log("ControllerGrounded: " + controllerGrounded);
        Debug.Log("Feet Position: " + feetPos);

        foreach (var hit in hits) {
            Debug.Log("HIT: " + hit.name + " | Layer: " + LayerMask.LayerToName(hit.gameObject.layer));

            if (hit.gameObject == gameObject) {
                Debug.LogWarning(" HITTING SELF (PLAYER COLLIDER)");
            }
        }
    }

    void OnDrawGizmos() {
        if (feetTransform == null) {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(feetTransform.position, radius);
    }
}