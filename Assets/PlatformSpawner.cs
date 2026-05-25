using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

public class PlatformSpawner : MonoBehaviour {

    [Header("Managers")]
    public MovingPlatformDown platformPrefab;

    [Space]
    [Header("Spawn Points")]
    [SerializeField] Vector3 point1;
    [SerializeField] Vector3 point2;

    // Private
    public ObjectPool<MovingPlatformDown> pooledPlatforms;
    BoxCollider spawnArea;

    void Awake() {
        platformPrefab.gameObject.SetActive(false);
        pooledPlatforms = new ObjectPool<MovingPlatformDown>(
            createFunc: () => Instantiate(platformPrefab, transform),
            actionOnGet: platform => SpawnPlatform(platform),
            actionOnRelease: platform => Despawn(platform),
            actionOnDestroy: platform => Destroy(platform),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100
        );
    }

    void Start() {
        spawnArea = GetComponent<BoxCollider>();
    }

    public IEnumerator SpawnRoutine(float rate, float speed, int maxPlatforms) {
        int spawnedPlatforms = 0;
        while (spawnedPlatforms++ < maxPlatforms) {
            pooledPlatforms.Get().OnSpawnPlatform(this, speed);
            yield return new WaitForSeconds(rate);
        }
        while (pooledPlatforms.CountActive > 0) {
            yield return null;
        }
    }

    Vector3 GetRandomPos() => Vector3.Lerp(point1 + transform.position, point2 + transform.position, Random.Range(0f, 1f));

    void SpawnPlatform(MovingPlatformDown platform) {
        platform.gameObject.SetActive(true);
        platform.transform.position = GetRandomPos();
    }

    void Despawn(MovingPlatformDown platform) {
        platform.gameObject.SetActive(false);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(point1 + transform.position, point2 + transform.position);
    }
}