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
        [field: SerializeField] public string name { get; private set; }
        [Tooltip("Animation Clip that will be used.")]
        [field: SerializeField] public AnimationClip animation { get; private set; }
        [Tooltip("Speed multiplier of original clip. (1 = default)")]
        [field: SerializeField] public float speed { get; private set; } = 1;
        [Tooltip("The amount this animation affects the model. (1 = default)")]
        [field: SerializeField] public float weight { get; private set; } = 1;
        [Tooltip("UNUSED - The transition period that the animation will mix with other animations.")]
        [field: SerializeField] public float crossFadeTime { get; private set; } = 0.2f;
        [Tooltip("The weight of the animation on the model over time. (use range x:0->1, y:0->1)")]
        [field: SerializeField] public AnimationCurve weightOverTime { get; private set; }
        public string clipName {
            get => name + (animation == null ? "" : animation.name);
        }
        public float length {
            get => animation != null ? animation.length : 1;
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
        public void Play(Animation anim) {
            if (anim == null || animation == null) return;
            anim.Play(clipName);
        }
        public void Blend(Animation anim, float weight) {
            if (anim == null || animation == null) return;
            anim.Blend(clipName, weight);
        }
        public void MixTransform(Animation anim, Transform transform) {
            if (anim == null || animation == null) return;
            if (transform != null)
                anim[clipName].AddMixingTransform(transform);
            Play(anim);
        }
        public void MixTransform((Animation component, Transform transform) modelInfo) => MixTransform(modelInfo.component, modelInfo.transform);
        #endregion
        public void Stop(Animation anim) {
            if (anim == null || animation == null) return;
            if (GetState(anim))
                anim.Stop(clipName);
        }
        public void PlayOnTimeline(Animation anim, Transform transform, float deltaTime, float weight = 1) {
            if (anim == null || animation == null) return;
            anim[clipName].time = deltaTime * anim[clipName].length;
        }
    }
}