using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor.EditorTools;

using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Freefall gamemode manager for when Kuhaku chases after Ellie and must dodge falling objects.
/// </summary>
[RequireComponent(typeof(LevelExit))]
public class FreefallGamemode : MonoBehaviour {
    [Tooltip("Set spawning object's parent.")]
    [SerializeField] Transform damageObjectParents;
    [Tooltip("Damaging object prefab.")]
    [SerializeField] FallGameplayTriggerDamage damageObjects;
    [Tooltip("Set the gamemode phases of different number of spawning objects and speed ranges.")]
    [SerializeField] List<Barrage> barrages;
    [Tooltip("Line point spawn bound 1.")]
    [SerializeField] Vector3 point1;
    [Tooltip("Line point spawn bound 2.")]
    [SerializeField] Vector3 point2;

    [Tooltip("Damaging object pool.")]
    public ObjectPool<FallGameplayTriggerDamage> pooledObjects;

    /// <summary>
    /// Setup damage object pool.
    /// </summary>
    void Awake() {
        pooledObjects = new ObjectPool<FallGameplayTriggerDamage>(
            createFunc: () => Instantiate(damageObjects, damageObjectParents),
            actionOnGet: damageObj => OnSpawn(damageObj),
            actionOnRelease: damageObj => Despawn(damageObj),
            actionOnDestroy: damageObj => Destroy(damageObj.gameObject),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100);
        StartCoroutine(Gameplay());
    }

    /// <summary>
    /// Draw gizmos to display spawn line.
    /// </summary>
    void OnDrawGizmos() {
        Gizmos.DrawLine(point1, point2);
    }

    /// <summary>
    /// Gameplay barrage of damage objects spawning against player. Then exit.
    /// </summary>
    public IEnumerator Gameplay() {
        foreach (var n in barrages) {
            yield return SpawnBarrage(n);
            yield return new WaitForSeconds(n.barrageGap);
        }
        GetComponent<LevelExit>().NextScene();
    }

    /// <summary>
    /// Spawn a barrage of items from a list of barrages.
    /// </summary>
    public IEnumerator SpawnBarrage(Barrage barrage) {
        for (int i = 0; i < UnityEngine.Random.Range(barrage.countMin, barrage.countMax); i++) {
            pooledObjects.Get().OnSpawn(this, UnityEngine.Random.Range(barrage.speedMin, barrage.speedMax));
            yield return new WaitForSeconds(UnityEngine.Random.Range(barrage.minInterval, barrage.maxInterval));
        }
    }

    /// <summary>
    /// Object pool on-spawn logic
    /// </summary>
    public void OnSpawn(FallGameplayTriggerDamage fallGameplayTriggerDamage) {
        fallGameplayTriggerDamage.gameObject.SetActive(true);
        fallGameplayTriggerDamage.transform.position = Vector3.Lerp(point1, point2, UnityEngine.Random.Range(0f, 1f));
    }
    /// <summary>
    /// Object pool despawn logic
    /// </summary>
    public void Despawn(FallGameplayTriggerDamage fallGameplayTriggerDamage) {
        fallGameplayTriggerDamage.gameObject.SetActive(false);
        fallGameplayTriggerDamage.transform.position = point1;
    }

    /// <summary>
    /// Data structure for barrage of objects.
    /// </summary>
    [Serializable]
    public struct Barrage {
        [Tooltip("")]
        public float minInterval;
        public float maxInterval;
        public int countMin;
        public int countMax;
        public float speedMin;
        public float speedMax;
        public float barrageGap;
    }
}
