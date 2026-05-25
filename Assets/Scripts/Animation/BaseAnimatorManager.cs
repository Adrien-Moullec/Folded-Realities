
using System;
using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    public abstract class BaseAnimatorManager : MonoBehaviour {

        protected Animator animator;

        protected (int layer, AnimatorFunctions state)[] layers = new (int layer, AnimatorFunctions)[] {
            (0, new AnimatorFunctions("",null,null,null,null)),
            (1, new AnimatorFunctions("",null,null,null,null))
        };

        void Awake() {
            animator = GetComponent<Animator>();
        }

        protected virtual bool CanStartAnimation((int hashCode, int layer) info) {
            if (layers[info.layer].state.currentState != "")
                return false;
            return true;
        }

        public void SetState(string stateName, float crossfade) {
            animator?.CrossFade(stateName, crossfade);
        }
        public IEnumerator InitiateOneOffAnimation(
            Action startF,
            Action<float> updateF,
            Action<AbilityAnimationEventData> eventF,
            Action endF,
            string animType,
            bool overrideState = true,
            float crossFade = 0
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
            if (animator.gameObject.activeSelf) animator.CrossFade(animType.ToString(), crossFade);
        }
        protected abstract int GetLayerInfo(string input);

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
                    layers[i].state.eventFunction?.Invoke(new AbilityAnimationEventData(animationEvent, stateInfo.normalizedTime % 1));
            }
        }
    }
}