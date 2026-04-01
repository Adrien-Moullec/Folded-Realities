using UnityEngine;

public class CameraFocus : MonoBehaviour {
    public Transform player;
    public float rotateSpeed = 6f;

    [Header("Offsets")]
    public Vector3 focusOffset = new Vector3(0f, 1.5f, -2f);

    [Header("Tilt")]
    public float tiltAmount = 10f;

    private Transform npc;
    private bool focusing = false;

    private Vector3 originalLocalPosition;
    private Quaternion originalRotation;

    void Start() {
        originalLocalPosition = transform.localPosition;
        originalRotation = transform.rotation;
    }

    public void FocusOn(Transform npcTarget) {
        npc = npcTarget;
        focusing = true;

        originalLocalPosition = transform.localPosition;
        originalRotation = transform.rotation;

        transform.localPosition = focusOffset;
    }

    public void StopFocus() {
        focusing = false;
        npc = null;

        transform.localPosition = originalLocalPosition;
        transform.rotation = originalRotation;
    }

    void LateUpdate() {
        if (!focusing || npc == null || player == null) {
            return;
        }

        Vector3 midpoint = (player.position + npc.position) / 2f;
        Vector3 lookPoint = midpoint + Vector3.up * 1.5f;

        Quaternion targetRotation = Quaternion.LookRotation(lookPoint - transform.position);

        targetRotation = Quaternion.Euler(
            targetRotation.eulerAngles.x + tiltAmount,
            targetRotation.eulerAngles.y,
            targetRotation.eulerAngles.z
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }
}