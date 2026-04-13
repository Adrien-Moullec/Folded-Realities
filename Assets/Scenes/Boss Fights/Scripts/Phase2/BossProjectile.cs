using UnityEngine;

public class BossProjectile : MonoBehaviour {
    public float speed = 8f;

    private Transform target;
    private bool hasTarget = false;

    public void SetTarget(Transform targetTransform) {
        target = targetTransform;
        hasTarget = true;
    }

    void Update() {
        if (!hasTarget || target == null) {
            return;
        }

        Vector3 targetPos = target.position + Vector3.up * 0.5f;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Platform")) {
            PlatformDissolve pd = other.GetComponent<PlatformDissolve>();

            if (pd != null) {
                pd.HitPlatform();
            }

            Destroy(gameObject);
        }
    }
}