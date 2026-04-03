using System;
using System.Collections;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD

using Unity.Mathematics;

using UnityEngine;

namespace AbilitySystem {
    public class ProjectileAbility : CooldownAbilitySO {

        [Header("Projectile Management")]
        [Tooltip("Projectile Gameobject that will get thrown.")]
        [SerializeField] protected PoolObject Projectile;
        [Tooltip("Animation that will play when throwing projectile.")]
        [SerializeField] protected AbilityAnimation projectileAnimation;
        [Tooltip("Number of projectile objects in current pool.")]
        [SerializeField] protected int poolSize;
=======
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
using UnityEngine.Pool;
using Unity.Mathematics;

using UnityEngine;
using Unity.VisualScripting;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Projectile Ability", menuName = MenuAssetNames.Projectiles + "/Generic Bullet")]
    public class ProjectileAbility : CooldownAbilitySO {

        [Header("Projectile Management")]
        [Tooltip("Info about the projectile pool.")]
        [SerializeField] protected ObjectPoolInfo projectilePoolInfo;
        [Tooltip("Animation that will play when throwing projectile.")]
        [SerializeField] protected AbilityAnimation projectileAnimation;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419

        [Space]
        [Header("Projectile Settings")]
        [Tooltip("The max speed of the projectile.")]
        [SerializeField] protected float projectileSpeed;
        [Tooltip("The max amount of time that projectiles will stay spawned in.")]
        [SerializeField] protected float lifetime;


        #region Data Setup
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        public override AbilityData AbilityDataSetup(EntityBody entityBody) {
            ProjectileData projectileData = new ProjectileData(charges, cooldown);
            projectileData.pooledProjectiles.SetPool(Projectile, poolSize);
            return projectileData;
        }
=======
        public override AbilityData AbilityDataSetup(EntityBody entityBody) => new ProjectileData(entityBody, projectilePoolInfo, charges, cooldown);
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
        public override AbilityData AbilityDataSetup(EntityBody entityBody) => new ProjectileData(entityBody, projectilePoolInfo, charges, cooldown);
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
        public override AbilityData AbilityDataSetup(EntityBody entityBody) => new ProjectileData(entityBody, projectilePoolInfo, charges, cooldown);
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
        public override AbilityData AbilityDataSetup(EntityBody entityBody) => new ProjectileData(entityBody, projectilePoolInfo, charges, cooldown);
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
        public override (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup() => new (AbilityAnimation, WrapMode)[] {
                (projectileAnimation,WrapMode.ClampForever)
            };
        #endregion

        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
            ProjectileData pd = (ProjectileData)data;
            float deltaTime = 0;
            PoolObject poolObject = pd.pooledProjectiles.SpawnFromPool(entityBody.modelPrefab.transform.position, quaternion.identity);

=======
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
            if (projectilePoolInfo.poolObject == null) yield break;
            ProjectileData pd = (ProjectileData)data;

            IPoolObjectAS projectile = pd.pooledProjectiles.Get();
            yield return new WaitForSeconds(2);
            pd.pooledProjectiles.Release(projectile);
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419

            yield return null;
        }
        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
            throw new System.NotImplementedException();
        }

        protected override void OnHold(EntityBody entityBody, CooldownData data) {
            throw new System.NotImplementedException();
        }

        protected override void RePress(EntityBody entityBody, CooldownData data) {
            throw new System.NotImplementedException();
=======
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
            throw new NotImplementedException();
        }

        protected override void OnHold(EntityBody entityBody, CooldownData data) {
            throw new NotImplementedException();
        }

        protected override void RePress(EntityBody entityBody, CooldownData data) {
            throw new NotImplementedException();
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
        }

        [Serializable]
        public class ProjectileData : CooldownData {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
            public ObjectPool pooledProjectiles;

            public ProjectileData(int charges, float cooldown) : base(charges, cooldown) { }
=======
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
            public ObjectPool<IPoolObjectAS> pooledProjectiles;
            public ProjectileData(EntityBody entityBody, ObjectPoolInfo poolInfo, int charges, float cooldown) : base(charges, cooldown) {
                pooledProjectiles = ObjectPoolInfo.CreateObjectPool(entityBody, poolInfo);
            }
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
=======
>>>>>>> 60d1f65c6cd3674953f3255e2389ed6c7150d419
        }
    }
}