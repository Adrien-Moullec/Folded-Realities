using UnityEngine;
using System.Collections;

public class ShredderBoss : MonoBehaviour {
    [Header("References")]
    public Transform player;

    public Transform shootPoint;

    public GameObject projectilePrefab;

    [Header("Attack")]
    public float attackInterval = 2f;

    public float projectileSpeed = 12f;

    [Header("Jiggle")]
    public float jiggleAmount = 0.1f;

    public float jiggleSpeed = 4f;

    [Header("Visuals")]
    public Renderer bossRenderer;

    Vector3 startPos;

    bool activeFight = false;

    bool jammed = false;

    int attacksDone = 0;

    bool phaseTwo = false;

    void Start() {
        startPos = transform.position;
    }

    void Update() {
        Jiggle();
    }

    void Jiggle() {
        transform.position =
            startPos
            + new Vector3(
                Mathf.Sin(
                    Time.time
                    * jiggleSpeed
                ) * jiggleAmount,
                0f,
                0f
            );
    }

    public void BeginFight() {
        activeFight = true;

        StartCoroutine(
            AttackLoop()
        );
    }

    IEnumerator AttackLoop() {
        while (activeFight) {
            if (!jammed) {
                ShootProjectile();

                attacksDone++;

                if (
                    attacksDone >= 10
                    && !phaseTwo
                ) {
                    phaseTwo = true;

                    attackInterval = 1f;

                    projectileSpeed = 18f;

                    StartCoroutine(
                        FlashRed()
                    );
                }
            }

            yield return new WaitForSeconds(
                attackInterval
            );
        }
    }

    void ShootProjectile() {
        GameObject proj =
            Instantiate(
                projectilePrefab,
                shootPoint.position,
                Quaternion.identity
            );

        BossMissile projectile =
            proj.GetComponent<BossMissile>();

        if (projectile != null) {
            projectile.SetTarget(player);

            projectile.speed =
                projectileSpeed;
        }
    }

    public void EnterJammedState() {
        jammed = true;

        StartCoroutine(
            FlashRed()
        );
    }

    IEnumerator FlashRed() {
        if (bossRenderer == null) {
            yield break;
        }

        for (int i = 0; i < 8; i++) {
            bossRenderer.material.color =
                Color.red;

            yield return new WaitForSeconds(
                0.15f
            );

            bossRenderer.material.color =
                Color.white;

            yield return new WaitForSeconds(
                0.15f
            );
        }
    }

    public void StartPhaseTwo() {
        jammed = false;

        phaseTwo = true;

        attackInterval = 1f;

        projectileSpeed = 18f;
    }
}