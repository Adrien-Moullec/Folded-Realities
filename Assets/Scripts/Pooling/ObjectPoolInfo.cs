using System;

using UnityEngine;
using UnityEngine.Pool;
namespace AbilitySystem {

    /// <summary>
    /// Object pool info for projectiles in an ability in ability system.
    /// </summary>
    [Serializable]
    public struct ObjectPoolInfo {
        [Tooltip("Pool object to use for projectile pool.")]
        public PoolObject poolObject;
        [Tooltip("Starting amount of projectiles to spawn.")]
        public int startingAmount;
        [Tooltip("Max amount of projectiles that can spawn.")]
        public int maxAmount;
        [Tooltip("Collection check for pooling information.")]
        public bool collectionCheck;

        /// <summary>
        /// Static function for creating a pool of IPoolObjectAS items for ability system abilities.
        /// </summary>
        /// <param name="entityBody"> reference entity information </param>
        /// <param name="poolInfo"> Pool information for generation </param>
        /// <returns></returns>
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

        /// <summary>
        /// Creating new objects for the IPoolObjectAS pool always requires instantiation.
        /// </summary>
        /// <typeparam name="T"> PoolObject inherited class </typeparam>
        /// <param name="obj"> PoolObject Object </param>
        /// <returns> Projectile object </returns>
        public static IPoolObjectAS CreateIPoolObject<T>(T obj) where T : PoolObject => GameObject.Instantiate(obj);
    }
}