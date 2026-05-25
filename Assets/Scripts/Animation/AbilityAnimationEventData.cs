
using System;

using UnityEngine;

namespace AbilitySystem {
    [Serializable]
    public struct AbilityAnimationEventData {
        public string a_functionName;
        public float a_float;
        public int a_int;
        public string a_string;
        public UnityEngine.Object a_Object;
        public float delta;

        public AbilityAnimationEventData(AnimationEvent animationEvent, float delta) {
            a_functionName = animationEvent.functionName;
            a_float = animationEvent.floatParameter;
            a_int = animationEvent.intParameter;
            a_string = animationEvent.stringParameter;
            a_Object = animationEvent.objectReferenceParameter;
            this.delta = delta;
        }
        public void Debug() =>
            UnityEngine.Debug.Log(
                "Function name: " + a_functionName +
                ", float: " + a_float +
                ", int: " + a_int +
                ", string: " + a_string +
                ", object: " + (a_Object == null ? "Null" : a_Object.name)
            );
    }
}