using UnityEngine;

public class NPCCatch : MonoBehaviour {
    public Transform player;
    public float catchDistance = 2f;
    public float groundHeight = 1.5f;
    public bool hasCaughtNPC = false;

    public BossPhaseManager bossManager;

    private bool caught = false;

    void Update() {
        if (caught || player == null) {
            return;
        }

        if (player.position.y <= groundHeight) {
            float dist = Vector3.Distance(player.position, transform.position);

            if (dist < catchDistance) {
                CatchNPC();
            }
        }
    }

    void CatchNPC() {
        caught = true;
        hasCaughtNPC = true;

        Debug.Log("NPC CAUGHT");

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts) {
            if (s != this) {
                s.enabled = false;
            }
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Collider col = GetComponent<Collider>();
        if (col != null) {
            col.enabled = false;
        }

        SlowFallOverride fall = player.GetComponent<SlowFallOverride>();
        if (fall != null) {
            fall.StopFalling();
        }

        transform.SetParent(player);
        transform.localPosition = new Vector3(0.5f, 1f, 0.5f);

        if (bossManager != null) {
            Debug.Log("Starting transition delay...");
            Invoke(nameof(StartBossPhaseDelayed), 2f);
        } else {
            Debug.LogError("BossManager NOT assigned!");
        }
    }

    void StartBossPhaseDelayed() {
        Debug.Log("Triggering Boss Phase NOW");
        bossManager.StartBossPhase();
    }
}