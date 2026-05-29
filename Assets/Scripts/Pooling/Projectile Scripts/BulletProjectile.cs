using UnityEngine;

using System;

namespace AbilitySystem {
    /// <summary>
    /// Bullet script for ability system projectiles.
    /// </summary>
    public class Bullet : Projectile {
        [Tooltip("Team reference.")]
        EntityTeam entityTeam = EntityTeam.None;
        [Tooltip("Direction of the bullet to travel.")]
        Vector3 direction;

        /// <summary>
        /// IPoolObjectAB reference to set up the ability projectile.
        /// </summary>
        public override void GetIPoolObj(EntityBody body) {
            gameObject.SetActive(true);
            entityBody = body;
            direction = -entityBody.bodyHolder.transform.forward;
            transform.position = body.bodyHolder.transform.position;
            transform.forward = direction;
            entityTeam = body == null ? EntityTeam.None : body.iAbility.GetEntityTeam;
        }

        /// <summary>
        /// Update the gameobject position over time so it shoots in a direction.
        /// </summary>
        void Update() {
            gameObject.transform.position -= direction * Time.deltaTime * 10;
        }

        /// <summary>
        /// Release back to the pool after hitting/expiring.
        /// </summary>
        public override void ReleaseIPoolObj(EntityBody body) {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Optional function for destroying the object.
        /// </summary>
        public override void OnDestroyIPoolObj(EntityBody body) { }

        /// <summary>
        /// Damage any IHealth entity on hit.
        /// </summary>
        public override void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out IHealth ihealth)) {
                ihealth.Damage(
                    new EntityDamage(
                        50,
                        entityBody,
                        entityBody.iAbility.GetEntityTeam,
                        EntityDamageType.Melee
                    )
                );
            } else if ((stopBulletLayer.value & (1 << other.gameObject.layer)) != 0)
                ReleaseIPoolObj(entityBody);
        }
    }
}