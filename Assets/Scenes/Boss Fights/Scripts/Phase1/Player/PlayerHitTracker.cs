using UnityEngine;

public class PlayerHitTracker : MonoBehaviour {
    public int maxHits = 3;

    int currentHits = 0;

    FallingKuhaku fallingPlayer;

    void Start() {
        fallingPlayer =
            GetComponent<
                FallingKuhaku
            >();
    }

    public void RegisterHit() {
        currentHits++;

        Debug.Log(
            "PLAYER HIT: "
            + currentHits
        );

        if (
            fallingPlayer != null
        ) {
            fallingPlayer
                .HitSlowdown();
        }

        if (
            currentHits >= maxHits
        ) {
            Debug.Log(
                "GAME OVER"
            );
        }
    }
}