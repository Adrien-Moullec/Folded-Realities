using System;
using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    [Serializable]
    public struct AnimationDeltaEventInfo {

        [Tooltip("Parts of the model that will be affected by the animation.")]
        public Transform affectedTransforms;
        [Tooltip("The model parts that will be affected by the animation.")]
        [Range(0, 1)] public float delta;
        public IEnumerator SetAction(Action action) => ActionToIenumerator(action);
        private static IEnumerator ActionToIenumerator(Action action) {
            action?.Invoke();
            yield return null;
        }
    }
}