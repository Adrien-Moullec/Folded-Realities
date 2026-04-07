using System;

using UnityEngine;
using UnityEngine.Pool;
using AbilitySystem;

namespace AbilitySystem {
    public abstract class PoolObject : MonoBehaviour, IPoolObjectAS {
        protected EntityBody entityBody;
        public abstract void GetIPoolObj(EntityBody body);
        public abstract void OnDestroyIPoolObj(EntityBody body);
        public abstract void ReleaseIPoolObj(EntityBody body);
    }
}