using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using Unity.VisualScripting;

using UnityEngine;

namespace AbilitySystem {
    [RequireComponent(typeof(Animator))]
    public class AnimatorManager : MonoBehaviour {
        private Animator animator;

        #region Parameters
        public const string deltaSpeed = "deltaSpeed";
        public const string deltaFall = "deltaFall";
        public const string isGrounded = "isGrounded";
        public const string fallState = "Fall";
        #endregion

        private Dictionary<int, AnimatorFunctions> animatorFunctionList = new();

        void Awake() {
            animator = GetComponent<Animator>();
        }
        #region States
        public void SetMovement(float dSpeed, float dFall, bool isGround) {
            animator.SetFloat(deltaSpeed, Mathf.Clamp01(dSpeed));
            animator.SetFloat(deltaFall, Mathf.Clamp01(dFall));
            animator.SetBool(isGrounded, isGround);
        }

        public IEnumerator InitiateOneOffAnimation(
            Action startF,
            Action<float> updateF,
            Action<AbilityAnimationData> eventF,
            Action endF,
            AnimationType animType,
            bool enableStateOverride = true
        ) {
            string state = animType switch {
                AnimationType.TransitionIn => "TransformIn",
                AnimationType.TransitionOut => "TransformOut",
                AnimationType.Attack1 => "Attack1",
                _ => null
            };

            if (string.IsNullOrEmpty(state))
                yield break;

            if (!TryStartAnimation(state, enableStateOverride, out int hashCode))
                yield break;

            // If already playing, stop it first
            if (animatorFunctionList.ContainsKey(hashCode)) {
                OnEndAnim(hashCode);
                Debug.Log("Force End state " + state);
            }

            Debug.Log("Play State " + state);

            animator.CrossFade(state, 0);

            animatorFunctionList[hashCode] =
                new AnimatorFunctions(startF, updateF, eventF, endF);

            //startF?.Invoke();

            // Wait until animation is removed
            while (animatorFunctionList.ContainsKey(hashCode))
                yield return null;
        }

        bool TryStartAnimation(string stateName, bool overrideState, out int hashCode) {
            hashCode = Animator.StringToHash(stateName);

            if (string.IsNullOrEmpty(stateName))
                return false;

            if (animatorFunctionList.ContainsKey(hashCode) && !overrideState) {
                Debug.Log("Animation already playing and override disabled");
                return false;
            }

            return true;
        }

        public void OnStartAnim(int hashCode) {
            animatorFunctionList[hashCode].startFunction?.Invoke();
            animatorFunctionList[hashCode].updateFunction?.Invoke(0);
        }
        public void OnUpdateAnim(int hashCode, float delta) {
            animatorFunctionList[hashCode].updateFunction?.Invoke(delta);
        }
        public void OnEndAnim(int hashCode) {
            animatorFunctionList[hashCode].updateFunction?.Invoke(1);
            animatorFunctionList[hashCode].endFunction?.Invoke();
            animatorFunctionList.Remove(hashCode);
        }
        public void ReceiveEvent(int hashCode, AnimationEvent animationEvent) {
            animatorFunctionList[hashCode].eventFunction?.Invoke(new AbilityAnimationData(animationEvent));
        }
        #endregion
    }
}