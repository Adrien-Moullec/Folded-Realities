using System;
using UnityEngine;

namespace AbilitySystem
{
    [Serializable]
    public struct AbilityAnimation
    {
        public AnimationClip animation;
        public PlayMode playMode;
        public float crossFadeTime;
        public float activateAbilityTime;
        public bool interruptCurrentAnimation;

        internal void PlayAnimation(Animation anim)
        {
            anim.CrossFade(animation.name, crossFadeTime, playMode);
        }
        internal void AddAnimation(Animation anim)
        {
            anim.AddClip(animation, animation.name);
        }
    }
}