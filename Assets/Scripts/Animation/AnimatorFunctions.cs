

using System;
using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    [Serializable]
    public struct AnimatorFunctions {
        public Action startFunction;
        public Action<AbilityAnimationData> eventFunction;
        public Action<float> updateFunction;
        public Action endFunction;

        public AnimatorFunctions(Action startF, Action<float> updateF, Action<AbilityAnimationData> eventF, Action endF) {
            startFunction = startF;
            updateFunction = updateF;
            eventFunction = eventF;
            endFunction = endF;
        }
    }
}