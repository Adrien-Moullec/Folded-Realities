

using UnityEngine;

namespace AbilitySystem {
    public abstract class Projectile : PoolObject {
        [SerializeField] protected LayerMask stopBulletLayer;
        public abstract void OnTriggerEnter(Collider other);
    }
}