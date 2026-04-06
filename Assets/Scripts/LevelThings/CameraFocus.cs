using UnityEngine;

public class CameraFocus : MonoBehaviour {
    public Transform player;
    public float followSpeed = 5f;
    public Vector3 offset;

    private Transform currentTarget;

    void Start() {
        currentTarget = player;
    }

    public void FocusOn(Transform target) {
        currentTarget = target;
    }

    public void ResetToPlayer() {
        currentTarget = player;
    }

    void LateUpdate() {
        if (currentTarget != null) {
            Vector3 desiredPosition = currentTarget.position
                                    + currentTarget.forward * offset.z
                                    + currentTarget.right * offset.x
                                    + Vector3.up * offset.y;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            transform.LookAt(currentTarget.position + Vector3.up * 1.5f);
        }
    }
}