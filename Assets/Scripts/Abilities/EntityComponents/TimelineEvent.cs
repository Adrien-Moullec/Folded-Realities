using System;

using UnityEngine;

namespace AbilitySystem {
    [Serializable]
    public struct TimelineEvent {
        public Animation component;
        public AbilityAnimation anim;
        public Transform transform;
        public Action action;
        public Func<bool> stopCondition;
        public float start;
        public float end;
        public bool reverse;
        public bool setTime;
        public TimelineEvent(Animation component, AbilityAnimation anim, float start, float end, bool setTime = false, Action action = null, Func<bool> stopCondition = null, bool reverse = false, Transform transform = null) {
            this.component = component;
            this.anim = anim;
            this.transform = transform;
            this.start = start;
            this.end = end;
            this.setTime = setTime;
            this.reverse = reverse;
            this.action = action;
            this.stopCondition = stopCondition;
        }
    }
}