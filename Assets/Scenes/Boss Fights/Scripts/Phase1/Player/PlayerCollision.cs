using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    private PlayerHitTracker tracker;

    public float hitCooldown = 0.5f;
    private bool canHit = true;

    void Start() {
        tracker = GetComponent<PlayerHitTracker>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (!canHit) return;

        if (hit.gameObject.CompareTag("Debris")) {
            canHit = false;
            Invoke(nameof(ResetHit), hitCooldown);

            if (tracker != null)
                tracker.RegisterHit();

            DebrisHit debris = hit.gameObject.GetComponent<DebrisHit>();
            if (debris != null)
                debris.Flash();
        }
    }

    void ResetHit() {
        canHit = true;
    }
}