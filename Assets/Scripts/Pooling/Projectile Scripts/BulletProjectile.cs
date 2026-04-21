using UnityEngine;

using System;

namespace AbilitySystem {
    public class Bullet : Projectile {
        EntityTeam entityTeam = EntityTeam.None;
        Vector3 direction;

        public override void GetIPoolObj(EntityBody body) {
            gameObject.SetActive(true);
            entityBody = body;
            direction = -entityBody.bodyHolder.transform.forward;
            transform.position = body.bodyHolder.transform.position;
            transform.forward = direction;
            entityTeam = body == null ? EntityTeam.None : body.iAbility.GetEntityTeam;
        }

        void Update() {
            gameObject.transform.position -= direction * Time.deltaTime * 10;
        }

        public override void ReleaseIPoolObj(EntityBody body) {
            gameObject.SetActive(false);
        }

        public override void OnDestroyIPoolObj(EntityBody body) {
            Debug.Log("AAAAAAAAAAAA");
        }

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