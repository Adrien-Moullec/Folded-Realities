using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

namespace AbilitySystem {
    [RequireComponent(typeof(Animator))]
    public class AnimatorManager : MonoBehaviour {
        private Animator animator;
        private Action<AbilityAnimationData> attackFunction;

        #region Parameters
        public const string deltaSpeed = "deltaSpeed";
        public const string deltaFall = "deltaFall";
        public const string isGrounded = "isGrounded";
        public const string fallState = "Fall";
        #endregion

        void Awake() {
            animator = GetComponent<Animator>();
        }
        #region States
        public void SetMovement(float dSpeed, float dFall, bool isGround) {
            animator.SetFloat(deltaSpeed, Mathf.Clamp01(dSpeed));
            animator.Play(fallState, 1, Mathf.Clamp01(dFall));
            animator.SetFloat(deltaFall, Mathf.Clamp01(dFall));
            animator.SetBool(isGrounded, isGround);
        }
        public IEnumerator InitiateOneOffAnimation(Action<AbilityAnimationData> animationFunction, AnimationType AttackNumber) {
            if (attackFunction == null)
                attackFunction = animationFunction;
            else {
                Debug.LogError("Trying to use 2 attacks at once, Staaahp! :(");
                yield break;
            }
            switch (AttackNumber) {
                case AnimationType.Attack1: animator.CrossFade("Attack1", 0.3f); break;
                case AnimationType.Attack2: break;
                case AnimationType.Attack3: break;
                case AnimationType.Attack4: break;
                case AnimationType.Attack5: break;
            }
            while (attackFunction != null)
                yield return null;
        }
        public void OnStartAnim() { }
        public void OnEndAnim() { attackFunction = null; }
        public void ReceiveEvent(AnimationEvent animationEvent) {
            AbilityAnimationData abilityAnimationData = new AbilityAnimationData(animationEvent);
            abilityAnimationData.Debug();
            attackFunction?.Invoke(abilityAnimationData);
            //if (animationEvent.stringParameter == "Attack")
            /// Important Values
            // animationEvent.animationState - Current playing state
            // animationEvent.animatorClipInfo - Animation clip info?
            // animationEvent.animatorStateInfo - state info

            // animationEvent.functionName - Function name passed
            // animationEvent.floatParameter - float passed
            // animationEvent.intParameter - Integer passed
            // animationEvent.stringParameter - String passed
            // animationEvent.objectReferenceParameter - no need for obj reference unless passing through multiple projectiles or something

            // animationEvent.isFiredByAnimator - will always be true
            // animationEvent.isFiredByLegacy - will always be false

            // animationEvent.messageOptions - 'RequireReceiver'?
            // animationEvent.time - delta? time frame of event
        }
        #endregion


        #region OUTTAKE
        /* 
        public void PlayAbility(AbilityAnimations animations) {
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            currentRoutine = StartCoroutine(PlaySequence(animations));
        }

        private IEnumerator PlaySequence(AbilityAnimations animations) {
            if (!string.IsNullOrEmpty(animations.startTrigger)) {
                animator.SetTrigger(animations.startTrigger);
                yield return WaitForAnimation();
            }

            if (animations.attackTriggers != null) {
                foreach (var trigger in animations.attackTriggers) {
                    animator.SetTrigger(trigger);
                    yield return WaitForAnimation();
                }
            }

            if (!string.IsNullOrEmpty(animations.endTrigger)) {
                animator.SetTrigger(animations.endTrigger);
            }
        }

        private IEnumerator WaitForAnimation() {
            yield return null;

            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            while (state.normalizedTime < 1f) {
                state = animator.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }
        }*/
        #endregion
    }
}