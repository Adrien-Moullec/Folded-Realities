using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Pool;

namespace AbilitySystem {

    [CreateAssetMenu(fileName = "Projectile Ability", menuName = MenuAssetNames.Projectiles + "/Generic Bullet")]
    public class ProjectileAbility : CooldownAbilitySO {

        [Header("Projectile Management")]
        [SerializeField] protected ObjectPoolInfo projectilePoolInfo;
        [SerializeField] protected AbilityAnimation projectileAnimation;

        [Header("Projectile Settings")]
        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected float lifetime = 2f;

        #region Data Setup

        public override AbilityData AbilityDataSetup(EntityBody entityBody) {
            return new ProjectileData(entityBody, projectilePoolInfo, charges, cooldown);
        }

        public override (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup() {
            return new (AbilityAnimation, WrapMode)[] {
                (projectileAnimation, WrapMode.ClampForever)
            };
        }

        #endregion

        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {

            ProjectileData pd = (ProjectileData)data;

            // Spawn projectile
            IPoolObjectAS projectile = pd.pooledProjectiles.Get();

            if (projectile is MonoBehaviour projMono) {

                // Use modelPrefab instead of transform (your system)
                projMono.transform.position = entityBody.modelPrefab.transform.position;
                projMono.transform.rotation = Quaternion.identity;

                // Apply movement if Rigidbody exists
                Rigidbody rb = projMono.GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.linearVelocity = entityBody.modelPrefab.transform.forward * projectileSpeed;
                }
            }

            yield return new WaitForSeconds(lifetime);

            pd.pooledProjectiles.Release(projectile);
        }

        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            return true;
        }

        protected override void OnHold(EntityBody entityBody, CooldownData data) { }

        protected override void RePress(EntityBody entityBody, CooldownData data) { }

        [Serializable]
        public class ProjectileData : CooldownData {

            public ObjectPool<IPoolObjectAS> pooledProjectiles;

            public ProjectileData(EntityBody entityBody, ObjectPoolInfo poolInfo, int charges, float cooldown)
                : base(charges, cooldown) {

                pooledProjectiles = ObjectPoolInfo.CreateObjectPool(entityBody, poolInfo);
            }
        }
    }
}