using UnityEngine;


namespace AbilitySystem {
    /// <summary>
    /// Custom animation state component for calling back to an animation manager with events.
    /// </summary>
    public class AnimationTracker : StateMachineBehaviour {
        [Tooltip("Animator Manager reference to call events too.")]
        private BaseAnimatorManager animatorManager;

        /// <summary>
        /// Event called on start of animation.
        /// </summary>
        /// <param name="animator"> Animator reference </param>
        /// <param name="stateInfo"> Animation state information </param>
        /// <param name="layerIndex"> Current layer playing on the animator </param>
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (animatorManager == null)
                animator.TryGetComponent(out animatorManager);

            animatorManager?.OnStartAnim(stateInfo.shortNameHash, layerIndex);
        }

        /// <summary>
        /// Event called every frame of the animation.
        /// </summary>
        /// <param name="animator"> Animator reference </param>
        /// <param name="stateInfo"> Animation state information </param>
        /// <param name="layerIndex"> Current layer playing on the animator </param>
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            animatorManager?.OnUpdateAnim(stateInfo.shortNameHash, layerIndex, stateInfo.normalizedTime);

        /// <summary>
        /// Event called on animation end.
        /// </summary>
        /// <param name="animator"> Animator reference </param>
        /// <param name="stateInfo"> Animation state information </param>
        /// <param name="layerIndex"> Current layer playing on the animator </param>
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animatorManager?.OnEndAnim(stateInfo.shortNameHash, layerIndex);
        }
    }
}