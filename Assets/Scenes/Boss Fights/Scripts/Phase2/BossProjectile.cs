using UnityEngine;

public class BossProjectile : MonoBehaviour {
    public float speed = 8f;

    private Vector3 target;
    private bool hasTarget = false;

    public void SetTarget(Vector3 targetPos) {
        target = targetPos;
        hasTarget = true;
    }

    void Update() {
        if (!hasTarget) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.1f) {
            Destroy(gameObject);
        }
    }
}