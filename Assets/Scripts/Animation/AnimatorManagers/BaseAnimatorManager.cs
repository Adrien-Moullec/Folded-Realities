
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

        protected bool CanStartAnimation((int hashCode, int layer) info) {
            if (layers[info.layer].state.currentState != "")
                return false;
            return true;
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
                    layers[i].state.eventFunction?.Invoke(new AbilityAnimationEventData(animationEvent));
            }
        }
    }
}