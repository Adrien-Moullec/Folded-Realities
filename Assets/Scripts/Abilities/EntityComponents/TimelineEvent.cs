using System;

using UnityEngine;

namespace AbilitySystem {
    [Serializable]
    public struct TimelineEvent {
        public Animation component;
        public AbilityAnimation anim;
        public Transform transform;
        public float start;
        public float end;
        public bool reverse;
        public TimelineEvent(Animation component, AbilityAnimation anim, float start, float end, bool reverse = false, Transform transform = null) {
            this.component = component;
            this.anim = anim;
            this.transform = transform;
            this.start = start;
            this.end = end;
            this.reverse = reverse;
        }
    }
}