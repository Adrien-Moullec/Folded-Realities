using UnityEngine;
using AbilitySystem;

public class BossProjectile : MonoBehaviour {

    [Header("Movement")]
    public float speed = 8f;

    public float lifeTime = 10f;

    public float overshootAmount = 8f;

    [Header("Damage")]
    public int damageAmount = 10;

    [Header("Fake Hit")]
    public float fakeHitRadius = 1.8f;

    Transform target;

    bool fired = false;

    bool playerAlreadyHit = false;

    Vector3 moveDirection;

    void Start() {

        Destroy(
            gameObject,
            lifeTime
        );
    }

    public void SetTarget(
        Transform targetTransform
    ) {

        target =
            targetTransform;

        if (target != null) {

            Vector3 targetPos =
                target.position +
                Vector3.up *
                overshootAmount;

            // ignore fake depth
            targetPos.z =
                transform.position.z;

            moveDirection =
                (
                    targetPos -
                    transform.position
                ).normalized;
        }

        fired = true;
    }

    void Update() {

        if (!fired) {
            return;
        }

        transform.position +=
            moveDirection *
            speed *
            Time.deltaTime;

        CheckFakePlayerHit();
    }

    void CheckFakePlayerHit() {

        if (
            playerAlreadyHit ||
            target == null
        ) {
            return;
        }

        Vector2 projectile2D =
            new Vector2(
                transform.position.x,
                transform.position.y
            );

        Vector2 player2D =
            new Vector2(
                target.position.x,
                target.position.y
            );

        float dist =
            Vector2.Distance(
                projectile2D,
                player2D
            );

        if (
            dist <= fakeHitRadius
        ) {

            PlayerAbilityController health =
                target.GetComponent<PlayerAbilityController>();

            if (health != null) {

                EntityDamage damage =
                    new EntityDamage();

                damage.amount =
                    damageAmount;

                health.Damage(
                    damage
                );
            }

            playerAlreadyHit = true;
        }
    }

    void OnTriggerEnter(
        Collider other
    ) {

        // ignore everything
        // fake hits only
    }
}