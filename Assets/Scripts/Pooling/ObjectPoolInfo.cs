using System;

using UnityEngine;
using UnityEngine.Pool;
namespace AbilitySystem {
    [Serializable]
    public struct ObjectPoolInfo {
        public PoolObject poolObject;
        public int startingAmount;
        public int maxAmount;
        public bool collectionCheck;
        public static ObjectPool<IPoolObjectAS> CreateObjectPool(EntityBody entityBody, ObjectPoolInfo poolInfo) =>
            new ObjectPool<IPoolObjectAS>(
                () => CreateIPoolObject(poolInfo.poolObject),
                (a) => a.GetIPoolObj(entityBody),
                (a) => a.ReleaseIPoolObj(entityBody),
                (a) => a.OnDestroyIPoolObj(entityBody),
                poolInfo.collectionCheck,
                poolInfo.startingAmount,
                poolInfo.maxAmount
            );

        public static IPoolObjectAS CreateIPoolObject<T>(T obj) where T : PoolObject => GameObject.Instantiate(obj);
    }
}