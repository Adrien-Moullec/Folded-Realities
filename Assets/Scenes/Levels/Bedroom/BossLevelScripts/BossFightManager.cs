using System.Collections;

using UnityEngine;

public class BossFightManager : MonoBehaviour {

    public static BossFightManager Instance;

    [Header("Boss")]
    public BossEnemy boss;

    [Header("Projectile Settings")]
    public float phase1ProjectileRate = 1f;
    public float phase1ProjectileSpeed = 8f;

    public float phase3ProjectileRate = 0.4f;
    public float phase3ProjectileSpeed = 14f;

    public float phase4ProjectileRate = 0.25f;
    public float phase4ProjectileSpeed = 18f;

    [Header("Platform Settings")]
    public PlatformSpawner platformSpawner;

    public float phase2SpawnRate = 1.5f;
    public int phase2MaxPlatforms = 5;
    public float phase2PlatformSpeed = 4f;
    public float phase2SuctionForce = 10f;

    public float phase4SpawnRate = 0.5f;
    public int phase4MaxPlatforms = 10;
    public float phase4PlatformSpeed = 8f;
    public float phase4SuctionForce = 18f;

    [Header("Phase Lengths")]
    public float phase1Length = 10f;
    public float phase2Length = 12f;
    public float phase3Length = 10f;

    [Header("Flash")]
    public Color phase2FlashColor = Color.red;
    public Color phase3FlashColor = Color.magenta;
    public Color phase4FlashColor = Color.yellow;

    Coroutine projectileRoutine;

    void Awake() {

        Instance = this;
    }

    void Start() {

        StartCoroutine(
            BossRoutine()
        );
    }

    IEnumerator BossRoutine() {

        // PHASE 1
        StartProjectilePhase(
            phase1ProjectileRate,
            phase1ProjectileSpeed
        );

        yield return new WaitForSeconds(
            phase1Length
        );

        // FLASH
        boss.angryColor =
            phase2FlashColor;

        yield return StartCoroutine(
            boss.FlashAngry()
        );

        // PHASE 2
        StopProjectileRoutine();

        StartPlatformPhase(
            phase2SpawnRate,
            phase2MaxPlatforms,
            phase2PlatformSpeed,
            phase2SuctionForce
        );

        yield return new WaitForSeconds(
            phase2Length
        );

        // FLASH
        boss.angryColor =
            phase3FlashColor;

        yield return StartCoroutine(
            boss.FlashAngry()
        );

        // PHASE 3
        StopPlatformPhase();

        StartProjectilePhase(
            phase3ProjectileRate,
            phase3ProjectileSpeed
        );

        yield return new WaitForSeconds(
            phase3Length
        );

        // FLASH
        boss.angryColor =
            phase4FlashColor;

        yield return StartCoroutine(
            boss.FlashAngry()
        );

        // PHASE 4
        StartPlatformPhase(
            phase4SpawnRate,
            phase4MaxPlatforms,
            phase4PlatformSpeed,
            phase4SuctionForce
        );

        StartProjectilePhase(
            phase4ProjectileRate,
            phase4ProjectileSpeed
        );
    }

    void StartProjectilePhase(
        float rate,
        float speed
    ) {

        StopProjectileRoutine();

        projectileRoutine =
            StartCoroutine(
                ProjectileRoutine(
                    rate,
                    speed
                )
            );
    }

    void StopProjectileRoutine() {

        if (
            projectileRoutine != null
        ) {

            StopCoroutine(
                projectileRoutine
            );

            projectileRoutine = null;
        }
    }

    IEnumerator ProjectileRoutine(
        float rate,
        float speed
    ) {

        while (true) {

            boss.Shoot(
                speed
            );

            yield return new WaitForSeconds(
                rate
            );
        }
    }

    void StartPlatformPhase(
        float spawnRate,
        int maxPlatforms,
        float platformSpeed,
        float suctionForce
    ) {

        if (
            platformSpawner == null
        ) {
            return;
        }

        platformSpawner.gameObject.SetActive(
            true
        );

        platformSpawner.spawnRate =
            spawnRate;

        platformSpawner.maxPlatforms =
            maxPlatforms;

        UpdatePlatforms(
            platformSpeed,
            suctionForce
        );
    }

    void StopPlatformPhase() {

        if (
            platformSpawner != null
        ) {

            platformSpawner.gameObject.SetActive(
                false
            );
        }
    }

    void UpdatePlatforms(
        float speed,
        float suction
    ) {

        MovingPlatformDown[] platforms =
            FindObjectsByType<MovingPlatformDown>(
                FindObjectsSortMode.None
            );

        foreach (
            MovingPlatformDown p
            in platforms
        ) {

            p.fallSpeed =
                speed;

            p.suctionForce =
                suction;

            p.bossTarget =
                boss.transform;
        }
    }
}