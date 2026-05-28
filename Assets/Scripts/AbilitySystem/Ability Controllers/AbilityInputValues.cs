using System;

using UnityEngine;

namespace AbilitySystem {
    /// <summary>
    /// List of input values to be called and adjusted to act as the middleman between inputs and abilities and movement
    /// </summary>
    [Serializable]
    public class AbilityControllerValues {

        #region Movement
        [Header("Movement Values")]

        #region InputValues
        [Tooltip("Get input ability values.")]
        public AbilityValues inputAbilityValues = new();
        [Tooltip("Check if alternative ability values should be used for situations where abilities control the movement.")]
        public bool isOverrideActive = false;
        [Tooltip("Override values where abilities control the movement.")]
        public AbilityValues overrideAbilityValues = new();
        #endregion

        #region Retrieve abilities
        [Tooltip("Get direction from input values based on if override is active.")]
        public Vector3 Direction { get => isOverrideActive ? overrideAbilityValues.direction : inputAbilityValues.direction; }
        [Tooltip("Get destination from input values based on if override is active.")]
        public Vector3 Destination { get => isOverrideActive ? overrideAbilityValues.destination : inputAbilityValues.destination; }
        [Tooltip("Get run status from input values based on if override is active.")]
        public bool IsRunning { get => isOverrideActive ? overrideAbilityValues.isRunning : inputAbilityValues.isRunning; }
        [Tooltip("Get crouch status from input values based on if override is active.")]
        public bool IsCrouching { get => isOverrideActive ? overrideAbilityValues.isCrouching : inputAbilityValues.isCrouching; }
        [Tooltip("Get acceleration status from input values based on if override is active.")]
        public bool IsAccelerating { get => isOverrideActive ? overrideAbilityValues.isAccelerating : inputAbilityValues.isAccelerating; }
        [Tooltip("Get movement type from input values based on if override is active.")]
        public MovementType MovementType { get => isOverrideActive ? overrideAbilityValues.movementType : inputAbilityValues.movementType; }
        [Tooltip("Return override status.")]
        public bool IsOverriding { get => isOverrideActive; }
        #endregion

        /// <summary>
        /// Set direction input in normal input or override
        /// </summary>
        public void SetDirection(Vector3 dir, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.SetDirection(dir);
            else overrideAbilityValues.SetDirection(dir);
        }
        /// <summary>
        /// Set direction input in normal input or override
        /// </summary>
        public void SetDirection(Vector2 dir, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.SetDirection(dir);
            else overrideAbilityValues.SetDirection(dir);
        }
        /// <summary>
        /// Set up direction input in normal input or override
        /// </summary>
        public void SetYDirection(float y, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.SetYDirection(y);
            else overrideAbilityValues.SetYDirection(y);
        }
        /// <summary>
        /// Set destination input in normal input or override
        /// </summary>
        public void SetDestination(Vector3 dest, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.destination = dest;
            else overrideAbilityValues.destination = dest;
        }
        /// <summary>
        /// Set run toggle in normal input or override
        /// </summary>
        public void SetRunToggle(bool isRunning, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.isRunning = isRunning;
            else overrideAbilityValues.isRunning = isRunning;
        }
        /// <summary>
        /// Set crouch toggle in normal input or override
        /// </summary>
        public void SetCrouchToggle(bool isCrouching, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.isCrouching = isCrouching;
            else overrideAbilityValues.isCrouching = isCrouching;
        }
        /// <summary>
        /// Set acceleration toggle in normal input or override
        /// </summary>
        public void SetAccelerateToggle(bool isAccelerating, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.isAccelerating = isAccelerating;
            else overrideAbilityValues.isAccelerating = isAccelerating;
        }
        /// <summary>
        /// Set movement type in normal input or override
        /// </summary>
        public void SetMovementTypeToggle(MovementType movementType, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.movementType = movementType;
            else overrideAbilityValues.movementType = movementType;
        }

        /// <summary>
        /// Set input default values
        /// </summary>
        internal void SetDefaultValues() => inputAbilityValues = new();

        #endregion

        [Header("Ability inputs.")]
        [Tooltip("If the primary ability button is being pressed.")]
        public bool isPrimaryAbility = false;
        [Tooltip("If the secondary ability button is being pressed.")]
        public bool isSecondaryAbility = false;
        [Tooltip("If the tertiary ability button is being pressed.")]
        public bool isTertiaryAbility = false;
    }

    public struct AbilityValues {
        [Tooltip("Current directional input for movement scripts.")]
        public Vector3 direction;
        [Tooltip("Destination value for auto-movement/enemies.")]
        public Vector3 destination;
        [Tooltip("Is Entity Running.")]
        public bool isRunning;

        [Tooltip("Is Entity Crouching.")]
        public bool isCrouching;

        [Tooltip("Switch between accelerating and not accelerating.")]
        public bool isAccelerating;
        [Tooltip("Set movement type.")]
        public MovementType movementType;

        [Tooltip("Set movement direction.")]
        public void SetDirection(Vector3 dir) => direction = dir;
        [Tooltip("Set movement direction.")]
        public void SetDirection(Vector2 dir) => direction = new Vector3(dir.x, direction.y, dir.y);
        [Tooltip("Set y movement direction.")]
        public void SetYDirection(float y) => direction.y = y;
    }
}