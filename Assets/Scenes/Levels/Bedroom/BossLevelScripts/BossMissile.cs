using UnityEngine;

public class BossMissile : MonoBehaviour {
    public float speed = 10f;

    Transform target;

    public void SetTarget(Transform t) {
        target = t;
    }

    void Update() {
        if (target == null) {
            Destroy(gameObject);

            return;
        }

        Vector3 dir =
            (
                target.position
                - transform.position
            ).normalized;

        transform.position +=
            dir
            * speed
            * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("PLAYER HIT");
        }

        Destroy(gameObject);
    }
}