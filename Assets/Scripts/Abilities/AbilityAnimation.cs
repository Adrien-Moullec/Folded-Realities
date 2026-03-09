using System;
using UnityEngine;

namespace AbilitySystem
{
    [Serializable]
    public class AbilityAnimation
    {
        public AnimationClip animation;
        public AnimationPlayStyle animationPlayStyle;
        public float speed = 1;
        public float weight = 1;
        public float crossFadeTime = 0.2f;
        [Range(0f, 1f)] public float abilityEventDelta = 1;
        public AnimationCurve weightOverTime;
    }
    public enum AnimationPlayStyle
    {
        SingleUse,
        Loop
    }
}