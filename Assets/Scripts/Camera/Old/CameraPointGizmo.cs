using UnityEngine;

[ExecuteAlways]
public class CameraPointGizmo : MonoBehaviour {
    public float fov = 60f;
    public float range = 10f;
    public Color gizmoColor = Color.cyan;

    void OnDrawGizmos() {
        Gizmos.color = gizmoColor;

        Matrix4x4 temp = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawFrustum(
            Vector3.zero,
            fov,
            range,
            0.1f,
            1f
        );

        Gizmos.DrawSphere(Vector3.zero, 0.1f);

        Gizmos.matrix = temp;
    }
}