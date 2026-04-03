

using UnityEngine;

namespace AbilitySystem {
    public abstract class Projectile : PoolObject {

        public abstract void OnTriggerEnter(Collider other);
    }
}