using System;
using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    /// <summary>
    /// Class for classic entity in the Ability System to call events.
    /// </summary>
    public class CharacterAnimatorManager : BaseAnimatorManager {

        #region Parameters
        [Tooltip("The parameter name for speed/maxspeed in blend tree animator controller.")]
        public const string deltaSpeed = "deltaSpeed";
        [Tooltip("The parameter name for fallspeed/maxfallspeed in blend tree animator controller.")]
        public const string deltaFall = "deltaFall";
        [Tooltip("The parameter name for whether an entity is grounded.")]
        public const string isGrounded = "isGrounded";
        [Tooltip("The parameter name for whether an entity is falling.")]
        public const string fallState = "Fall";
        #endregion

        #region States
        /// <summary>
        /// Callable movement function to set the blend tree variables in animator.
        /// </summary>
        public void SetMovement(float dSpeed, float dFall, bool isGround) {
            animator?.SetFloat(deltaSpeed, Mathf.Clamp01(dSpeed));
            animator?.SetFloat(deltaFall, Mathf.Clamp01(dFall));
            animator?.SetBool(isGrounded, isGround);
        }
        /// <summary>
        /// Set the movement state in animator controller.
        /// </summary>
        public void SetMovementState() {
            animator.CrossFade("Grounded", 0);
        }

        /// <summary>
        /// Get the correct corresponding layer with the animation name.
        /// </summary>
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