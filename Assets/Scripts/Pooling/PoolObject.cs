using System;

using UnityEngine;
using UnityEngine.Pool;

namespace AbilitySystem {
    public abstract class PoolObject2 : MonoBehaviour, IPoolObjectAS {
        protected EntityBody entityBody;
        public abstract void GetIPoolObj(EntityBody body);
        public abstract void OnDestroyIPoolObj(EntityBody body);
        public abstract void ReleaseIPoolObj(EntityBody body);
    }
}