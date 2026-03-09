using System;
using UnityEngine;

namespace AbilitySystem
{
    [Serializable]
    public class AbilityAnimation
    {
        public AnimationClip animation;
        public AnimationPlayStyle animationPlayStyle;
        public PlayMode playMode;
        public float speed = 1;
        public float weight = 1;
        public float crossFadeTime = 0.2f;
        public float activateAbilityTime;

        internal void PlayAnimation(Animation anim)
        {
            anim.CrossFade(animation.name, crossFadeTime, playMode);
        }
        internal void AddAnimation(Animation anim)
        {
            anim.AddClip(animation, animation.name);
        }
    }
    public enum AnimationPlayStyle
    {
        SingleUse,
        Loop
    }
}