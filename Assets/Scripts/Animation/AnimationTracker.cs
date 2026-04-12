

using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;


namespace AbilitySystem {
    public class AnimationTracker : StateMachineBehaviour {
        private AnimatorManager animatorManager;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (animatorManager == null)
                animator.TryGetComponent(out animatorManager);

            animatorManager?.OnStartAnim(stateInfo.shortNameHash, layerIndex);
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animatorManager?.OnUpdateAnim(stateInfo.shortNameHash, layerIndex, stateInfo.normalizedTime);
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animatorManager?.OnEndAnim(stateInfo.shortNameHash, layerIndex);
        }
    }
}