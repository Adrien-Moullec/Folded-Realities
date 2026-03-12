using System;
using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    [Serializable]
    public struct DeltaEvent {
        public IEnumerator action;
        public float deltaTime;
        public DeltaEvent(IEnumerator action, float deltaTime) {
            this.action = action;
            this.deltaTime = deltaTime;
        }
        public DeltaEvent(Action action, float deltaTime) {
            this.action = ActionToIenumerator(action);
            this.deltaTime = deltaTime;
        }
        private static IEnumerator ActionToIenumerator(Action action) {
            action?.Invoke();
            yield return null;
        }
    }
}