

using UnityEngine;

namespace AbilitySystem {
    /// <summary>
    /// Projectile base class for entity projectile pool.
    /// </summary>
    public abstract class Projectile : PoolObject {
        [Tooltip("Target layers")]
        [SerializeField] protected LayerMask stopBulletLayer;

        /// <summary>
        /// Trigger enter is mandatory for a projectile.
        /// </summary>
        public abstract void OnTriggerEnter(Collider other);
    }
}