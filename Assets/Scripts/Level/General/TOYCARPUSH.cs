using UnityEngine;

public class TOYCARPUSH : MonoBehaviour {

    public Transform pointA;
    public Transform pointB;

    public float moveSpeed = 2f;
    public float smoothSpeed = 10f;

    private float t = 0f;
    private Vector3 pathDir;
    private Vector3 targetPos;
    private float fixedY;

    void Start() {
        pathDir = (pointB.position - pointA.position).normalized;

        fixedY = transform.position.y;

        Vector3 closest = GetClosestPointOnLine(transform.position);
        t = GetT(closest);

        targetPos = closest;

        Vector3 rot = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rot.y, 0f);
    }

    public void Push(Vector3 inputMove) {

        float dot = Vector3.Dot(inputMove, pathDir);

        if (Mathf.Abs(dot) < 0.1f) return;

        t += dot * moveSpeed * Time.deltaTime;
        t = Mathf.Clamp01(t);

        Vector3 pos = Vector3.Lerp(pointA.position, pointB.position, t);
        pos.y = fixedY;

        targetPos = pos;
    }

    void Update() {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
    }

    Vector3 GetClosestPointOnLine(Vector3 pos) {
        Vector3 ap = pos - pointA.position;
        float d = Vector3.Dot(ap, pathDir);
        d = Mathf.Clamp(d, 0f, Vector3.Distance(pointA.position, pointB.position));
        return pointA.position + pathDir * d;
    }

    float GetT(Vector3 pos) {
        float total = Vector3.Distance(pointA.position, pointB.position);
        float current = Vector3.Distance(pointA.position, pos);
        return current / total;
    }

    void LateUpdate() {
        Vector3 rot = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rot.y, 0f);
    }
}