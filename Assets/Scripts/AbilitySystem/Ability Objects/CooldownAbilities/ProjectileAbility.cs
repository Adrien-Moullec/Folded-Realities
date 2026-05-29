using System;
using System.Collections;

using UnityEngine.Pool;
using UnityEngine;

namespace AbilitySystem {

    /// <summary>
    /// Base projectile ability class that uses a pooled projectile to shoot items with preconfigured behaviours
    /// </summary>
    [CreateAssetMenu(fileName = "Projectile Ability", menuName = MenuAssetNames.Projectiles + "/Generic Bullet")]
    public class ProjectileAbility : CooldownAbilitySO {

        [Header("Projectile Management")]
        [Tooltip("Info about the projectile pool.")]
        [SerializeField] protected ObjectPoolInfo projectilePoolInfo;

        [Space]
        [Header("Projectile Settings")]
        [Tooltip("The max speed of the projectile.")]
        [SerializeField] protected float projectileSpeed;
        [Tooltip("The lifetime of the projectile in seconds.")]
        [SerializeField] protected float projectileLifetime = 2;
        [Tooltip("The max amount of time that projectiles will stay spawned in.")]
        [SerializeField] protected float shootInterval = 1f;
        [Tooltip("The time spawned between burst shots.")]
        [SerializeField] protected float burstInterval = 0.25f;
        [Tooltip("The amount of burst shots.")]
        [SerializeField, Min(1)] protected int burstAmount = 5;


        /// <summary>
        /// Setup base projectile data.
        /// </summary>
        public override AbilityData AbilityDataSetup(EntityBody entityBody) => new ProjectileData(entityBody, projectilePoolInfo, charges, cooldown);

        /// <summary>
        /// Wait for animation to end before ending ability.
        /// </summary>
        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
            yield return AttackAnimation(entityBody, data, AnimationType.Attack1);
        }

        /// <summary>
        /// Event that happens every frame.
        /// </summary>
        public override void FrameEvent(EntityBody entityBody, AbilityData abData) {
            base.FrameEvent(entityBody, abData);
        }

        /// <summary>
        /// Shoot a projectile event
        /// </summary>
        IEnumerator Shoot(EntityBody entityBody, AbilityData abilityData) {
            ProjectileData pd = (ProjectileData)abilityData;

            /// Shoot a number of missiles
            for (int i = 0; i < burstAmount; i++) {
                entityBody.iAbility.GetAbilityController.StartCoroutine(Projectile(pd));
                yield return new WaitForSeconds(burstInterval);
            }

            yield return new WaitForSeconds(shootInterval - burstInterval);
        }

        /// <summary>
        /// Setup projectile
        /// </summary>
        IEnumerator Projectile(ProjectileData pd) {
            IPoolObjectAS projectile = pd.pooledProjectiles.Get();
            yield return new WaitForSeconds(projectileLifetime);
            pd.pooledProjectiles.Release(projectile);
        }
        /// <summary>
        /// Shoot projectile on animation event
        /// </summary>
        public override void AnimationEvent(AbilityAnimationEventData animationData, EntityBody entityBody, AbilityData abilityData) => entityBody.iAbility.GetAbilityController.StartCoroutine(Shoot(entityBody, abilityData));

        #region Unused
        protected override void OnHold(EntityBody entityBody, CooldownData data) { }
        protected override void OnPressWhileUsing(EntityBody entityBody, CooldownData data) { }
        #endregion


        /// <summary>
        /// Base projectile ability data 
        /// </summary>
        [Serializable]
        public class ProjectileData : CooldownData {
            [Tooltip("Pool of projectile objects for prolonged ability use.")]
            public ObjectPool<IPoolObjectAS> pooledProjectiles;

            /// <summary>
            /// Create a projectile pool and setup variable data.
            /// </summary>
            public ProjectileData(EntityBody entityBody, ObjectPoolInfo poolInfo, int charges, float cooldown) : base(charges, cooldown) {
                pooledProjectiles = ObjectPoolInfo.CreateObjectPool(entityBody, poolInfo);
            }
        }
    }
}