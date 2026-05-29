using UnityEngine;

namespace AbilitySystem {
    /// <summary>
    /// Pool object base script for setting pool behaviours and containing reference entity information.
    /// </summary>
    public abstract class PoolObject : MonoBehaviour, IPoolObjectAS {
        protected EntityBody entityBody;
        public abstract void GetIPoolObj(EntityBody body);
        public abstract void OnDestroyIPoolObj(EntityBody body);
        public abstract void ReleaseIPoolObj(EntityBody body);
    }
}