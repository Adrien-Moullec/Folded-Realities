using UnityEngine;

public class BossStartTrigger : MonoBehaviour {
    public ShredderBoss boss;

    bool triggered = false;

    void OnTriggerEnter(
        Collider other
    ) {
        if (triggered) {
            return;
        }

        if (
            other.CompareTag(
                "Player"
            )
        ) {
            triggered = true;

            Debug.Log(
                "BOSS FIGHT STARTED"
            );

            boss.enabled = true;

            boss.BeginFight();
        }
    }
}