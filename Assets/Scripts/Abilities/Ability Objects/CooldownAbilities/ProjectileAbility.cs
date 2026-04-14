using System;
using System.Collections;

using UnityEngine.Pool;
using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Projectile Ability", menuName = MenuAssetNames.Projectiles + "/Generic Bullet")]
    public class ProjectileAbility : CooldownAbilitySO {

        [Header("Projectile Management")]
        [Tooltip("Info about the projectile pool.")]
        [SerializeField] protected ObjectPoolInfo projectilePoolInfo;

        [Space]
        [Header("Projectile Settings")]
        [Tooltip("The max speed of the projectile.")]
        [SerializeField] protected float projectileSpeed;
        [Tooltip("The max amount of time that projectiles will stay spawned in.")]
        [SerializeField] protected float shootInterval = 1f;
        [SerializeField] protected float burstInterval = 0.25f;
        [SerializeField, Min(1)] protected int burstAmount = 5;
        [SerializeField] bool automaticShooting = false;
        [SerializeField] protected float lifetime;


        #region Data Setup
        public override AbilityData AbilityDataSetup(EntityBody entityBody) => new ProjectileData(entityBody, projectilePoolInfo, charges, cooldown);
        #endregion

        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
            Debug.Log("StartEventProjectile");
            //if (projectilePoolInfo.poolObject == null || (data.isHoldingInput && !automaticShooting)) yield break;
            ProjectileData pd = (ProjectileData)data;

            yield return AttackAnimation(entityBody, data, AnimationType.Attack1);
        }
        public override void FrameEvent(AbilityData abData) {
            base.FrameEvent(abData);
            Debug.Log("PASS EVENT " + ((CooldownData)abData).cooldownDelta);
        }
        IEnumerator Shoot(EntityBody entityBody, AbilityData abilityData) {

            ProjectileData pd = (ProjectileData)abilityData;
            for (int i = 0; i < burstAmount; i++) {
                entityBody.iAbility.GetAbilityController.StartCoroutine(Projectile(pd));
                yield return new WaitForSeconds(burstInterval);
            }

            yield return new WaitForSeconds(shootInterval - burstInterval);
        }
        IEnumerator Projectile(ProjectileData pd) {
            IPoolObjectAS projectile = pd.pooledProjectiles.Get();
            yield return new WaitForSeconds(2);
            pd.pooledProjectiles.Release(projectile);
        }

        protected override void OnHold(EntityBody entityBody, CooldownData data) {

        }

        protected override void OnPressWhileUsing(EntityBody entityBody, CooldownData data) {

        }

        public override void AnimationEvent(AbilityAnimationEventData animationData, EntityBody entityBody, AbilityData abilityData) {
            Debug.Log("EVENT - Projectile");
            entityBody.iAbility.GetAbilityController.StartCoroutine(Shoot(entityBody, abilityData));
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