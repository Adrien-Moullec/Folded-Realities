using System;
using System.Collections;

using UnityEngine;

public class BossFightManager : MonoBehaviour {

    public static BossFightManager Instance;

    [Header("Boss")]
    public BossEnemy boss;
    [SerializeField] ShredderAnimatorManager animationManager;

    [SerializeField] BossFightSection[] bossFight;

    [Header("Platform Settings")]
    public PlatformSpawner platformSpawner;

    float phaseIntervalLength = 0.5f;
    bool isFin = false;

    void Awake() =>
        Instance = this;


    void Start() =>
        StartCoroutine(BossRoutine());


    IEnumerator BossRoutine() {
        isFin = false;
        yield return animationManager.InitiateOneOffAnimation(
            null,
            null,
            null,
            () => isFin = true,
            ShredderAnim.WakeUp.ToString()
        );
        while (!isFin) yield return null;

        foreach (var n in bossFight) {
            if (n.skipPhase) continue;

            yield return ProjectileRoutine(n.spitPhase.rate, n.spitPhase.speed, n.spitPhase.maxSpawn);
            yield return new WaitForSeconds(phaseIntervalLength);
            animationManager.SetState(ShredderAnim.SpinAttack.ToString(), 0.2f);
            yield return platformSpawner.SpawnRoutine(n.suckPhase.rate, n.suckPhase.speed, n.suckPhase.maxSpawn);
            yield return new WaitForSeconds(phaseIntervalLength);
            animationManager.SetState(ShredderAnim.Idle.ToString(), 0.2f);


            yield return StartCoroutine(boss.FlashAngry(n.color));
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
        GameplaySystem.instance.LoadScene(GameplayScenes.Bedroom);
    }
    IEnumerator ProjectileRoutine(float rate, float speed, int spawnCount) {
        int shootCount = 0;
        boss.isMove = true;
        while (shootCount < spawnCount) {
            isFin = false;
            yield return animationManager.InitiateOneOffAnimation(
                null,
                null,
                (x) => { boss.Shoot(speed); Debug.Log("Shoot"); },
                () => isFin = true,
                ShredderAnim.Spit.ToString(),
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

[Serializable]
public struct BossFightSection {
    public Color color;
    public bool skipPhase;
    public SpitPhase spitPhase;
    public SuckPhase suckPhase;
}
[Serializable]
public struct SpitPhase {
    public float rate;
    public float speed;
    public int maxSpawn;
}
[Serializable]
public struct SuckPhase {
    public float rate;
    public float speed;
    public int maxSpawn;
    public float suction;
}
/// flash Colour
/// 
/// --- Spit ---
/// rate
/// speed
/// amount
/// 
/// 
/// --- Suck ---
/// rate
/// speed
/// amount
/// suction
/// 