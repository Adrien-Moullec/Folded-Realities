using System;
using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    [RequireComponent(typeof(Animator))]
    public class AnimatorManager : BaseAnimatorManager {

        #region Parameters
        public const string deltaSpeed = "deltaSpeed";
        public const string deltaFall = "deltaFall";
        public const string isGrounded = "isGrounded";
        public const string fallState = "Fall";
        #endregion
        #region States
        public void SetMovement(float dSpeed, float dFall, bool isGround) {
            animator?.SetFloat(deltaSpeed, Mathf.Clamp01(dSpeed));
            animator?.SetFloat(deltaFall, Mathf.Clamp01(dFall));
            animator?.SetBool(isGrounded, isGround);
        }
        public void SetMovementState() {
            animator.CrossFade("Grounded", 0);
        }

        public IEnumerator InitiateOneOffAnimation(
            Action startF,
            Action<float> updateF,
            Action<AbilityAnimationEventData> eventF,
            Action endF,
            string animType,
            bool overrideState
        ) {

            (int stateHashCode, int layer) info = (Animator.StringToHash(animType), GetLayerInfo(animType));

            if (!CanStartAnimation(info) && !overrideState)
                yield break;

            // If already playing, stop it first
            if (layers[info.layer].state.currentState != "") {
                OnEndAnim(info.stateHashCode, 0);
                yield return null;
            }

            layers[info.layer].state = new AnimatorFunctions(animType.ToString(), startF, updateF, eventF, endF);
            if (animator.gameObject.activeSelf) animator.CrossFade(animType.ToString(), 0);
        }

        protected override int GetLayerInfo(string input) {
            switch (input) {
                case nameof(AnimationType.Attack1): return 1;
                case nameof(AnimationType.TransformIn): return 0;
                case nameof(AnimationType.TransformOut): return 0;
                case nameof(AnimationType.Death): return 0;
                default: return -1;
            }
        }
        #endregion
    }
}