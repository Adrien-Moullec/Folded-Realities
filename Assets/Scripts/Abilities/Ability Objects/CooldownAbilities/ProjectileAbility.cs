using System;
using System.Collections;

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

        [Space]
        [Header("Projectile Settings")]
        [Tooltip("The max speed of the projectile.")]
        [SerializeField] protected float projectileSpeed;
        [Tooltip("The max amount of time that projectiles will stay spawned in.")]
        [SerializeField] protected float lifetime;


        #region Data Setup
        public override AbilityData AbilityDataSetup(EntityBody entityBody) {
            ProjectileData projectileData = new ProjectileData(charges, cooldown);
            projectileData.pooledProjectiles.SetPool(Projectile, poolSize);
            return projectileData;
        }
        public override (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup() => new (AbilityAnimation, WrapMode)[] {
                (projectileAnimation,WrapMode.ClampForever)
            };
        #endregion

        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
            ProjectileData pd = (ProjectileData)data;
            float deltaTime = 0;
            PoolObject poolObject = pd.pooledProjectiles.SpawnFromPool(entityBody.modelPrefab.transform.position, quaternion.identity);


            yield return null;
        }
        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            throw new System.NotImplementedException();
        }

        protected override void OnHold(EntityBody entityBody, CooldownData data) {
            throw new System.NotImplementedException();
        }

        protected override void RePress(EntityBody entityBody, CooldownData data) {
            throw new System.NotImplementedException();
        }

        [Serializable]
        public class ProjectileData : CooldownData {
            public ObjectPool pooledProjectiles;

            public ProjectileData(int charges, float cooldown) : base(charges, cooldown) { }
        }
    }
}