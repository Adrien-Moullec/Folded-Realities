using UnityEngine;
using AbilitySystem;

public class MovingPlatformDown : MonoBehaviour {

    [Header("Movement")]
    public float fallSpeed = 3f;

    [Header("Respawn")]
    public float despawnOffset = 15f;

    [Header("Damage")]
    public int damageAmount = 10;

    public float damageCooldown = 1f;

    [Header("Boss Suction")]
    public Transform bossTarget;

    public float suctionForce = 8f;

    public float suctionStartDistanceBelowPlayer = 3f;

    [Header("Fake Thickness")]
    public float visualHitPadding = 1.5f;

    [HideInInspector]
    public PlatformSpawner spawner;

    Transform player;

    float damageTimer;

    bool suctionActive = false;

    void Start() {

        GameObject playerObj =
            GameObject.FindGameObjectWithTag(
                "Player"
            );

        if (playerObj != null) {

            player =
                playerObj.transform;
        }
    }

    void Update() {

        if (player == null) {
            return;
        }

        // normal falling
        transform.position +=
            Vector3.down *
            fallSpeed *
            Time.deltaTime;

        // only start suction AFTER passing player
        if (
            !suctionActive &&
            transform.position.y <
            player.position.y -
            suctionStartDistanceBelowPlayer
        ) {

            suctionActive = true;
        }

        // suck toward boss
        if (
            suctionActive &&
            bossTarget != null
        ) {

            Vector3 dir =
                (
                    bossTarget.position -
                    transform.position
                ).normalized;

            transform.position +=
                dir *
                suctionForce *
                Time.deltaTime;
        }

        // fake damage zone
        if (
            IsPlayerUnderPlatform()
        ) {

            PlayerAbilityController health =
                player.GetComponent<PlayerAbilityController>();

            if (
                health != null &&
                Time.time > damageTimer
            ) {

                EntityDamage damage =
                    new EntityDamage();

                damage.amount =
                    damageAmount;

                health.Damage(
                    damage
                );

                damageTimer =
                    Time.time +
                    damageCooldown;
            }
        }

        // cleanup
        if (
            transform.position.y <
            player.position.y -
            despawnOffset
        ) {

            if (
                spawner != null
            ) {

                spawner.PlatformDestroyed();
            }

            Destroy(
                gameObject
            );
        }
    }

    bool IsPlayerUnderPlatform() {

        float horizontalDistance =
            Mathf.Abs(
                player.position.x -
                transform.position.x
            );

        bool closeHorizontally =
            horizontalDistance < 2.2f;

        bool playerBelow =
            player.position.y <
            transform.position.y;

        bool closeVertically =
            transform.position.y -
            player.position.y <
            2.5f +
            visualHitPadding;

        return
            closeHorizontally &&
            playerBelow &&
            closeVertically;
    }
}