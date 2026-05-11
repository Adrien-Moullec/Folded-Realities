using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    private PlayerHitTracker tracker;

    public float hitCooldown = 0.5f;

    private bool canHit = true;

    void Start() {
        tracker =
            GetComponent<
                PlayerHitTracker
            >();
    }

    void OnTriggerEnter(
        Collider other
    ) {
        if (!canHit) {
            return;
        }

        if (
            other.CompareTag(
                "Debris"
            )
        ) {
            canHit = false;

            Invoke(
                nameof(ResetHit),
                hitCooldown
            );

            if (tracker != null) {
                tracker.RegisterHit();
            }

            DebrisHit debris =
                other.GetComponent<
                    DebrisHit
                >();

            if (debris != null) {
                debris.Flash();
            }
        }
    }

    void ResetHit() {
        canHit = true;
    }
}