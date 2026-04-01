using UnityEngine;

public class CameraZoom : MonoBehaviour {
    public float normalDistance = -6f;
    public float zoomDistance = -3.5f;
    public float speed = 5f;

    private float targetZ;

    void Start() {
        targetZ = normalDistance;
    }

    public void ZoomIn() {
        targetZ = zoomDistance;
    }

    public void ZoomOut() {
        targetZ = normalDistance;
    }

    void LateUpdate() {
        Vector3 pos = transform.localPosition;
        pos.z = Mathf.Lerp(pos.z, targetZ, speed * Time.deltaTime);
        transform.localPosition = pos;
    }
}