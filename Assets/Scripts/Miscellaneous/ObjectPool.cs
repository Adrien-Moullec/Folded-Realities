using System;
using System.Collections.Generic;

using UnityEngine;

namespace AbilitySystem {
    [Serializable]
    public struct ObjectPool {
        public Queue<PoolObject> pooledObjects;
        public void SetPool(PoolObject component, int poolSize) {
            Queue<PoolObject> objectPool = new Queue<PoolObject>();
            for (int i = 0; i < poolSize; i++) {
                PoolObject poolObj = GameObject.Instantiate(component);
                poolObj.gameObject.SetActive(false);
                objectPool.Enqueue(poolObj.GetComponent<PoolObject>());
                poolObj.Setup();
            }
        }

        public PoolObject SpawnFromPool(Vector3 position, Quaternion rotation) {

            PoolObject poolObj = pooledObjects.Dequeue();
            pooledObjects.Enqueue(poolObj);
            poolObj.OnStart();

            poolObj.gameObject.SetActive(true);
            poolObj.transform.position = position;
            poolObj.transform.rotation = rotation;

            return poolObj;
        }

        public void ReturnToPool(PoolObject poolObj) {
            poolObj.OnEnd();
            poolObj.gameObject.SetActive(false);
        }
    }
}