using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Pool;

[RequireComponent(typeof(LevelExit))]
public class FreefallGamemode : MonoBehaviour {
    [SerializeField] Transform damageObjectParents;
    [SerializeField] FallGameplayTriggerDamage damageObjects;
    [SerializeField] Barrage[] barrages;
    [SerializeField] Vector3 point1;
    [SerializeField] Vector3 point2;
    [SerializeField] float DespawnHeight = 10;
    [SerializeField, Min(0.1f)] float Speed = 1;

    ObjectPool<FallGameplayTriggerDamage> pooledObjects;

    void Awake() {
        pooledObjects = new ObjectPool<FallGameplayTriggerDamage>(
            createFunc: () => Instantiate(damageObjects, damageObjectParents),
            actionOnGet: damageObj => OnSpawn(damageObj),
            actionOnRelease: damageObj => damageObj.gameObject.SetActive(false),
            actionOnDestroy: damageObj => Destroy(damageObj.gameObject),
            collectionCheck: true,   // An Editor-only check that determines if an instance is returned back to the pool. Throws an exception if the instance is already in the pool.
            defaultCapacity: 10,
            maxSize: 100);
        StartCoroutine(Gameplay());
    }

    void OnDrawGizmos() {
        Gizmos.DrawLine(point1, point2);
    }
    public IEnumerator Gameplay() {
        foreach (var n in barrages) {
            yield return SpawnBarrage(n);
            yield return new WaitForSeconds(n.barrageGap);
        }
        GetComponent<LevelExit>().NextScene();
    }
    public IEnumerator SpawnBarrage(Barrage barrage) {
        float t; Vector3 spawnPos;
        for (int i = 0; i < UnityEngine.Random.Range(barrage.countMin, barrage.countMax); i++) {
            t = UnityEngine.Random.Range(0f, 1f);
            spawnPos = Vector3.Lerp(point1, point2, t);
            pooledObjects.Get().OnSpawn(this, spawnPos, UnityEngine.Random.Range(barrage.speedMin, barrage.speedMax));
            yield return new WaitForSeconds(UnityEngine.Random.Range(barrage.minInterval, barrage.maxInterval));
        }
    }

    public void OnSpawn(FallGameplayTriggerDamage fallGameplayTriggerDamage) {
        fallGameplayTriggerDamage.gameObject.SetActive(true);
    }
    public void Despawn(FallGameplayTriggerDamage fallGameplayTriggerDamage) {
        pooledObjects.Release(fallGameplayTriggerDamage);
    }

    [Serializable]
    public struct Barrage {
        public float minInterval;
        public float maxInterval;
        public int countMin;
        public int countMax;
        public float speedMin;
        public float speedMax;
        public float barrageGap;
    }
}
