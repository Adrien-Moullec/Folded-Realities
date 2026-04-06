using UnityEngine;

namespace AbilitySystem {

    public class Bullet : PoolObject2 {

        private Vector3 direction;

        public override void GetIPoolObj(EntityBody body) {
            gameObject.SetActive(true);

            entityBody = body;

            direction = entityBody.modelPrefab.transform.forward;

            transform.position = body.bodyHolder.transform.position;
            transform.rotation = Quaternion.identity;
        }

        public override void ReleaseIPoolObj(EntityBody body) {
            gameObject.SetActive(false);
        }

        public override void OnDestroyIPoolObj(EntityBody body) {
            Debug.Log("Destroyed Bullet");
        }

        void Update() {
            transform.position += direction * Time.deltaTime * 10f;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out IHealth ihealth)) {
                ihealth.Damage(50);
            }
        }
    }
}