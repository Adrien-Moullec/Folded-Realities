using System;
using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimatorManager : BaseAnimatorManager {

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