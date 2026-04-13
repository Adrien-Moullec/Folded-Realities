using System;
using System.Collections;

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

        private (int layer, AnimatorFunctions state)[] layers = new (int layer, AnimatorFunctions)[] {
            (0, new AnimatorFunctions("",null,null,null,null)),
            (1, new AnimatorFunctions("",null,null,null,null))
        };

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
            Action<AbilityAnimationEventData> eventF,
            Action endF,
            AnimationType animType,
            bool overrideState
        ) {

            (int stateHashCode, int layer) info =
                (Animator.StringToHash(animType.ToString()),
                animType switch {
                    AnimationType.Attack1 => 1,
                    AnimationType.TransformIn => 0,
                    AnimationType.TransformOut => 0,
                    _ => -1,
                });

            if (!CanStartAnimation(info))
                yield break;

            // If already playing, stop it first
            if (layers[info.layer].state.currentState != "") {
                OnEndAnim(info.stateHashCode, 0);
                yield return null;
            }

            layers[info.layer].state = new AnimatorFunctions(animType.ToString(), startF, updateF, eventF, endF);
            animator.CrossFade(animType.ToString(), 0);
        }

        bool CanStartAnimation((int hashCode, int layer) info) {

            if (layers[info.layer].state.currentState != "") { // || animator.HasState(0, hashCode)) {
                Debug.Log("Animation already playing and override disabled " + layers[info.layer].state.currentState);
                return false;
            }
            return true;
        }

        public void OnStartAnim(int hashCode, int layerIndex) {
            layers[layerIndex].state.startFunction?.Invoke();
            layers[layerIndex].state.updateFunction?.Invoke(0);
        }
        public void OnUpdateAnim(int hashCode, int layerIndex, float delta) {
            layers[layerIndex].state.updateFunction?.Invoke(delta);
        }
        public void OnEndAnim(int hashCode, int layerIndex) {
            layers[layerIndex].state.updateFunction?.Invoke(1);
            layers[layerIndex].state.endFunction?.Invoke();
            layers[layerIndex].state = new AnimatorFunctions("", null, null, null, null);
        }
        public void ReceiveEvent(AnimationEvent animationEvent) {

            for (int i = 0; i < animator.layerCount; i++) {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(i);

                if (Animator.StringToHash(layers[i].state.currentState) == stateInfo.shortNameHash)
                    layers[i].state.eventFunction?.Invoke(new AbilityAnimationEventData(animationEvent));
            }
        }
        #endregion
    }
}