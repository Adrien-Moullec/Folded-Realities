using System;
using System.Collections;

using UnityEngine;

namespace AbilitySystem {

    /// <summary> Ability Animation
    /// This class stores all the needed data to create fluid animations
    /// </summary>
    [Serializable]
    public class AbilityAnimation {
        #region Animation Information
        [Tooltip("Unique name that will be stored in the Animation Component. (name + animation.name)")]
        public string name;
        [Tooltip("Animation Clip that will be used.")]
        public AnimationClip animation;
        [Tooltip("Speed multiplier of original clip. (1 = default)")]
        public float speed = 1;
        [Tooltip("The amount this animation affects the model. (1 = default)")]
        public float weight = 1;
        [Tooltip("UNUSED - The transition period that the animation will mix with other animations.")]
        public float crossFadeTime = 0.2f;
        [Tooltip("The weight of the animation on the model over time. (use range x:0->1, y:0->1)")]
        public AnimationCurve weightOverTime;
        public string clipName {
            get {
                string s = name + (animation == null ? "" : animation.name);
                return s;
            }
        }
        #endregion

        #region Set Animation Data
        public AnimationState GetState(Animation anim) => anim[clipName];
        public float SetWeight(Animation anim, float weight) => anim[clipName].weight = weight;
        public void Setup(Animation animComponent, WrapMode wrapMode) {
            if (animComponent == null || animation == null) return;

            animation.legacy = true;
            animComponent.AddClip(animation, clipName);
            animComponent[clipName].speed = speed;
            animComponent[clipName].wrapMode = wrapMode;
        }
        #endregion

        #region Play Modes
        public void Play(Animation anim) => anim.Play(clipName);
        public void Blend(Animation anim, float weight) => anim.Blend(clipName, weight);
        public void MixTransform(Animation anim, Transform transform) {
            if (anim == null) return;
            if (transform != null)
                anim[clipName].AddMixingTransform(transform);
            Play(anim);
        }
        public void MixTransform((Animation component, Transform transform) modelInfo) => MixTransform(modelInfo.component, modelInfo.transform);
        #endregion
    }
}