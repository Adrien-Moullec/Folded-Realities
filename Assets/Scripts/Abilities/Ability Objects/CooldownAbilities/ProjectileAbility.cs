using System;
using System.Collections;
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

        [Space]
        [Header("Projectile Settings")]
        [Tooltip("The max speed of the projectile.")]
        [SerializeField] protected float projectileSpeed;
        [Tooltip("The max amount of time that projectiles will stay spawned in.")]
        [SerializeField] protected float lifetime;


        #region Data Setup
        public override AbilityData AbilityDataSetup(EntityBody entityBody) => new ProjectileData(entityBody, projectilePoolInfo, charges, cooldown);
        public override (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup() => new (AbilityAnimation, WrapMode)[] {
                (projectileAnimation,WrapMode.ClampForever)
            };
        #endregion

        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
            if (projectilePoolInfo.poolObject == null) yield break;
            ProjectileData pd = (ProjectileData)data;

            IPoolObjectAS projectile = pd.pooledProjectiles.Get();
            yield return new WaitForSeconds(2);
            pd.pooledProjectiles.Release(projectile);

            yield return null;
        }
        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            throw new NotImplementedException();
        }

        protected override void OnHold(EntityBody entityBody, CooldownData data) {
            throw new NotImplementedException();
        }

        protected override void RePress(EntityBody entityBody, CooldownData data) {
            throw new NotImplementedException();
        }

        [Serializable]
        public class ProjectileData : CooldownData {
            public ObjectPool<IPoolObjectAS> pooledProjectiles;
            public ProjectileData(EntityBody entityBody, ObjectPoolInfo poolInfo, int charges, float cooldown) : base(charges, cooldown) {
                pooledProjectiles = ObjectPoolInfo.CreateObjectPool(entityBody, poolInfo);
            }
        }
    }
}