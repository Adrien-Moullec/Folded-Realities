using UnityEngine;

using System;

namespace AbilitySystem {
    public class Bullet : Projectile {
        Vector3 direction;

        public override void GetIPoolObj(EntityBody body) {
            gameObject.SetActive(true);
            entityBody = body;
            direction = entityBody.modelPrefab.transform.forward;
            transform.position = body.bodyHolder.transform.position;
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
                ihealth.Damage(50);
            }
        }
    }
}