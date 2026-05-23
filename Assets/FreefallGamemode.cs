using System.Collections;

using UnityEngine;
using UnityEngine.Pool;

public class FreefallGamemode : MonoBehaviour {
    [SerializeField] Transform damageObjectParents;
    [SerializeField] GameObject damageObjects;
    [SerializeField] Vector3 point1;
    [SerializeField] Vector3 point2;
    [SerializeField] float DespawnHeight = 10;

    ObjectPool<GameObject> pooledObjects;

    void Awake() {
        pooledObjects = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(damageObjects, damageObjectParents),
            actionOnGet: gameObject => gameObject.SetActive(true),
            actionOnRelease: gameObject => gameObject.SetActive(false),
            actionOnDestroy: gameObject => Destroy(gameObject),
            collectionCheck: true,   // An Editor-only check that determines if an instance is returned back to the pool. Throws an exception if the instance is already in the pool.
            defaultCapacity: 10,
            maxSize: 100);
    }
    public void FloatEvent(float value) {
        GameObject s = pooledObjects.Get();
        s.transform.position = Vector3.Lerp(point1, point2, value);
        StartCoroutine(Despawn(s));
    }

    IEnumerator Despawn(GameObject poolObj) {
        while (poolObj.transform.position.y < point1.y + DespawnHeight) {
            poolObj.transform.position += new Vector3(0, 1f * Time.deltaTime, 0);
            yield return null;
        }
        pooledObjects.Release(poolObj);
    }

    void OnDrawGizmos() {
        Gizmos.DrawLine(point1, point2);
    }
}
