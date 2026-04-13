using UnityEngine;

public class BossTrigger : MonoBehaviour {
    public BossPhaseManager bossManager;
    public NPCCatch npcCatch;

    private bool triggered = false;

    void OnTriggerEnter(Collider other) {
        if (triggered) return;

        if (!other.CompareTag("Player")) return;

        if (npcCatch == null || !npcCatch.hasCaughtNPC) {
            Debug.Log("NPC not caught yet");
            return;
        }

        triggered = true;

        Debug.Log("BOSS TRIGGERED");

        bossManager.StartBossPhase();
    }
}