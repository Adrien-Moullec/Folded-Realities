
using System;

using UnityEngine;

namespace AbilitySystem {
    /// <summary>
    /// The received data from an animation state event during animator controller playtime.
    /// </summary>
    [Serializable]
    public struct AbilityAnimationEventData {
        [Tooltip("Called function name.")]
        public string a_functionName;
        [Tooltip("Called float from event.")]
        public float a_float;
        [Tooltip("Called int from event.")]
        public int a_int;
        [Tooltip("Called string from event.")]
        public string a_string;
        [Tooltip("Called object from event.")]
        public UnityEngine.Object a_Object;
        [Tooltip("Called delta value from event time.")]
        public float delta;

        /// <summary>
        /// Animation event data setup
        /// </summary>
        /// <param name="animationEvent"> Default animator class animation data </param>
        /// <param name="delta"> 0->1 current progression of animation </param>
        public AbilityAnimationEventData(AnimationEvent animationEvent, float delta) {
            a_functionName = animationEvent.functionName;
            a_float = animationEvent.floatParameter;
            a_int = animationEvent.intParameter;
            a_string = animationEvent.stringParameter;
            a_Object = animationEvent.objectReferenceParameter;
            this.delta = delta;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Editor functionality for testing.
        /// </summary>
        public void Debug() =>
            UnityEngine.Debug.Log(
                "Function name: " + a_functionName +
                ", float: " + a_float +
                ", int: " + a_int +
                ", string: " + a_string +
                ", object: " + (a_Object == null ? "Null" : a_Object.name)
            );
#endif
    }
}