using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Pool;

[RequireComponent(typeof(LevelExit))]
public class FreefallGamemode : MonoBehaviour {
    [SerializeField] Transform damageObjectParents;
    [SerializeField] FallGameplayTriggerDamage damageObjects;
    [SerializeField] List<Barrage> barrages;
    [SerializeField] Vector3 point1;
    [SerializeField] Vector3 point2;
    [SerializeField] float DespawnHeight = 10;
    [SerializeField, Min(0.1f)] float Speed = 1;

    public ObjectPool<FallGameplayTriggerDamage> pooledObjects;
    int spawnCount = 0;

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
        for (int i = 0; i < UnityEngine.Random.Range(barrage.countMin, barrage.countMax); i++) {
            pooledObjects.Get().OnSpawn(this, UnityEngine.Random.Range(barrage.speedMin, barrage.speedMax));
            yield return new WaitForSeconds(UnityEngine.Random.Range(barrage.minInterval, barrage.maxInterval));
        }
    }

    public void OnSpawn(FallGameplayTriggerDamage fallGameplayTriggerDamage) {
        fallGameplayTriggerDamage.gameObject.SetActive(true);
        fallGameplayTriggerDamage.transform.position = Vector3.Lerp(point1, point2, UnityEngine.Random.Range(0f, 1f));
    }
    public void Despawn(FallGameplayTriggerDamage fallGameplayTriggerDamage) {
        fallGameplayTriggerDamage.gameObject.SetActive(false);
        fallGameplayTriggerDamage.transform.position = point1;
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
