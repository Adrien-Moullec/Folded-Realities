

using System;
using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    [Serializable]
    public class AnimatorFunctions {
        public string currentState;
        public Action startFunction;
        public Action<AbilityAnimationEventData> eventFunction;
        public Action<float> updateFunction;
        public Action endFunction;

        public AnimatorFunctions(string currentState, Action startF, Action<float> updateF, Action<AbilityAnimationEventData> eventF, Action endF) {
            this.currentState = currentState;
            startFunction = startF;
            updateFunction = updateF;
            eventFunction = eventF;
            endFunction = endF;
        }
    }
}