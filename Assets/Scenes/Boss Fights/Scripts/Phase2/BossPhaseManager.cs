using UnityEngine;

public class BossPhaseManager : MonoBehaviour {
    public GameObject fallingSection;
    public GameObject bossSection;

    public Transform player;
    public Transform bossStartPoint;

    public void StartBossPhase() {
        Debug.Log("STARTING BOSS PHASE");

        if (fallingSection != null) {
            fallingSection.SetActive(false);
        }

        if (bossSection != null) {
            bossSection.SetActive(true);
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) {
            cc.enabled = false;
        }

        player.position = bossStartPoint.position;

        if (cc != null) {
            cc.enabled = true;
        }
    }
}