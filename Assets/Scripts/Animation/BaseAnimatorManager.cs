
using System;
using System.Collections;

using UnityEngine;

namespace AbilitySystem {

    /// <summary>
    /// Base abstract class for all animator managers in the project.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public abstract class BaseAnimatorManager : MonoBehaviour {

        [Tooltip("Optional debug option for editing.")]
        [SerializeField] protected bool debug = false;
        [Tooltip("Animator reference.")]
        protected Animator animator;

        /// <summary>
        /// Base animator layers that exist on the animator controllers, most ofter for top and bottom animations of a humanoid model.
        /// </summary>
        protected (int layer, AnimatorFunctions state)[] layers = new (int layer, AnimatorFunctions)[] {
            (0, new AnimatorFunctions("",null,null,null,null)),
            (1, new AnimatorFunctions("",null,null,null,null))
        };

        /// <summary>
        /// Retrieve animator
        /// </summary>
        void Awake() {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Reset event animation.
        /// </summary>
        /// <param name="layer"> The controller layer to reset. </param>
        public virtual void CleanLayer(int layer) {
            layers[layer].state = new AnimatorFunctions("", null, null, null, null);
        }

        /// <summary>
        /// Check for if an animation can be played or not.
        /// </summary>
        protected virtual bool CanStartAnimation((int hashCode, int layer) info) {
            if (layers[info.layer].state.currentState != "")
                return false;
            return true;
        }

        /// <summary>
        /// Set an animation state with crossfade time.
        /// </summary>
        /// <param name="stateName"> Animation state name </param>
        /// <param name="crossfade"> Crossfade time </param>
        public void SetState(string stateName, float crossfade) {
            animator?.CrossFade(stateName, crossfade);
        }

        /// <summary>
        /// Try start an animation and subscribe events to the animation.
        /// </summary>
        public IEnumerator InitiateOneOffAnimation(
            Action startF,
            Action<float> updateF,
            Action<AbilityAnimationEventData> eventF,
            Action endF,
            string animType,
            bool overrideState = true,
            float crossFade = 0
        ) {
            /// Base info for the state in hash and layer id.
            (int stateHashCode, int layer) info = (Animator.StringToHash(animType), GetLayerInfo(animType));

            /// Break away if the animation is impossible to play.
            if (!CanStartAnimation(info) && !overrideState)
                yield break;

            // If already playing, stop it first
            if (layers[info.layer].state.currentState != "") {
                OnEndAnim(info.stateHashCode, 0);
                yield return null;
            }

            /// Play animation and subscribe events
            layers[info.layer].state = new AnimatorFunctions(animType.ToString(), startF, updateF, eventF, endF);
            if (animator.gameObject.activeSelf) animator.CrossFade(animType.ToString(), crossFade);
        }

        /// <summary>
        /// Get the animation state layer based on the string input.
        /// </summary>
        protected abstract int GetLayerInfo(string input);

        /// <summary>
        /// On animation start, play start event.
        /// </summary>
        public void OnStartAnim(int hashCode, int layerIndex) {
            layers[layerIndex].state.startFunction?.Invoke();
            layers[layerIndex].state.updateFunction?.Invoke(0);
            if (debug) Debug.Log("START");
        }
        /// <summary>
        /// On animation update, play start event.
        /// </summary>
        public void OnUpdateAnim(int hashCode, int layerIndex, float delta) {
            layers[layerIndex].state.updateFunction?.Invoke(delta);
        }
        /// <summary>
        /// On animation end, play start event.
        /// </summary>
        public void OnEndAnim(int hashCode, int layerIndex) {
            layers[layerIndex].state.updateFunction?.Invoke(1);
            layers[layerIndex].state.endFunction?.Invoke();
            OnEnd(layerIndex);
        }
        /// <summary>
        /// On animation end, reset values and allow for additional settings in inheriting scripts.
        /// </summary>
        protected virtual void OnEnd(int layerIndex) {
            layers[layerIndex].state = new AnimatorFunctions("", null, null, null, null);
        }
        /// <summary>
        /// On animation event received, play custom subscribed event.
        /// </summary>
        public void ReceiveEvent(AnimationEvent animationEvent) {
            for (int i = 0; i < animator.layerCount; i++)
                ActOnAnimatorStateInfoReceiveEvent(animationEvent, animator.GetCurrentAnimatorStateInfo(i), i);
        }
        /// <summary>
        /// Received event function.
        /// </summary>
        protected virtual void ActOnAnimatorStateInfoReceiveEvent(AnimationEvent animationEvent, AnimatorStateInfo stateInfo, int layer) {
            if (Animator.StringToHash(layers[layer].state.currentState) == stateInfo.shortNameHash)
                layers[layer].state.eventFunction?.Invoke(new AbilityAnimationEventData(animationEvent, stateInfo.normalizedTime % 1));
        }
    }
}