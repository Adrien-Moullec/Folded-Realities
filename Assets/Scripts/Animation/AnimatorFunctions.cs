

using System;

using UnityEngine;

namespace AbilitySystem {

    /// <summary>
    /// Store for animation functions
    /// </summary>
    [Serializable]
    public struct AnimatorFunctions {
        [Tooltip("Name of current animator state playing.")]
        public string currentState;
        [Tooltip("Event played at the start of the animation.")]
        public Action startFunction;
        [Tooltip("Event played when animation events are called.")]
        public Action<AbilityAnimationEventData> eventFunction;
        [Tooltip("Event played when animation updates are called.")]
        public Action<float> updateFunction;
        [Tooltip("Event played at the end of the animation.")]
        public Action endFunction;

        /// <summary>
        /// Initiate animation values
        /// </summary>
        public AnimatorFunctions(string currentState, Action startF, Action<float> updateF, Action<AbilityAnimationEventData> eventF, Action endF) {
            this.currentState = currentState;
            startFunction = startF;
            updateFunction = updateF;
            eventFunction = eventF;
            endFunction = endF;
        }
    }
}