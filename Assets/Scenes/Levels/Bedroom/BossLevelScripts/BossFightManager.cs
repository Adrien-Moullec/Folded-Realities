using System;
using System.Collections;
using System.Linq;

using UnityEngine;

[RequireComponent(typeof(LevelExit))]
public class BossFightManager : MonoBehaviour {
    //Adrienne updated with animations
    public static BossFightManager Instance;

    [Header("Boss")]
    public BossEnemy boss;
    [SerializeField] ShredderAnimatorManager animationManager;

    [SerializeField] BossFightSection[] bossFight;
    // Platform spawning controller
    [Header("Platform Settings")]
    public PlatformSpawner platformSpawner;

    float phaseIntervalLength = 0.5f;
    bool isFin = false;

    void Awake() =>
        Instance = this;


    void Start() =>
        // Starts full boss fight sequence
        StartCoroutine(BossRoutine());


    IEnumerator BossRoutine() {
        // Starts boss in idle animation
        animationManager.SetState(ShredderAnim.Idle.ToString(), 0.2f);
        // Loops through boss fight phases
        yield return new WaitForSeconds(2);

        int count = 0;
        foreach (var n in bossFight) {
            if (n.skipPhase) {
                count++;
                continue;
            }
            count++;
            // Projectile Attack Phase
            yield return ProjectileRoutine(n.spitPhase.rate, n.spitPhase.speed, n.spitPhase.maxSpawn);
            yield return new WaitForSeconds(phaseIntervalLength);
            // Platform Suction Phase
            animationManager.SetState(ShredderAnim.SpinAttack.ToString(), 0.2f);
            yield return platformSpawner.SpawnRoutine(n.suckPhase.rate, n.suckPhase.speed, n.suckPhase.maxSpawn);
            yield return new WaitForSeconds(phaseIntervalLength);
            animationManager.SetState(ShredderAnim.Idle.ToString(), 0.2f);


            yield return StartCoroutine(boss.FlashAngry(n.color));
            if (count < bossFight.Count(x => !x.skipPhase)) {
                isFin = false;
                yield return animationManager.InitiateOneOffAnimation(
                    null,
                    null,
                    null,
                    () => isFin = true,
                    ShredderAnim.Hit.ToString(),
                    crossFade: 0.05f
                );
                while (!isFin) yield return null;
            }
        }
        // Boss Death Sequence
        isFin = false;
        yield return animationManager.InitiateOneOffAnimation(
            null,
            null,
            null,
            () => isFin = true,
            ShredderAnim.Death.ToString(),
            crossFade: 0.05f
        );
        while (!isFin) yield return null;
        yield return new WaitForSeconds(2);
        GetComponent<LevelExit>().NextScene();
    }
    IEnumerator ProjectileRoutine(float rate, float speed, int spawnCount) {
        // Tracks number of projectiles fired
        int shootCount = 0;
        boss.isMove = true;
        while (shootCount < spawnCount) {
            isFin = false;
            yield return animationManager.InitiateOneOffAnimation(
                null,
                null,
                // Fires projectile during animation event
                (x) => { Debug.Log("Function received"); boss.Shoot(speed); isFin = true; },
                () => isFin = true,
                ShredderAnim.SpitCharge.ToString(),
                crossFade: 0.05f
            );
            while (!isFin) yield return null;

            shootCount++;
            yield return new WaitForSeconds(rate);
        }
        boss.isMove = false;
        yield return boss.MoveToCentre();
    }
}
// BossFightSection
[Serializable]
public struct BossFightSection {
    public Color color;
    public bool skipPhase;
    public SpitPhase spitPhase;
    public SuckPhase suckPhase;
}// SpitPhase
[Serializable]
public struct SpitPhase {
    public float rate;
    public float speed;
    public int maxSpawn;
}// SuckPhase
[Serializable]
public struct SuckPhase {
    public float rate;
    public float speed;
    public int maxSpawn;
    public float suction;
}