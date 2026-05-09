using System;

using UnityEngine;

namespace AbilitySystem {
    [Serializable]
    public class AbilityControllerValues {

        #region Movement
        [Header("Movement Values")]

        #region InputValues
        public AbilityValues inputAbilityValues = new();
        public bool isOverrideActive = false;
        public AbilityValues overrideAbilityValues = new();
        #endregion

        #region Retrieve abilities
        public Vector3 Direction { get => isOverrideActive ? overrideAbilityValues.direction : inputAbilityValues.direction; }
        public Vector3 Destination { get => isOverrideActive ? overrideAbilityValues.destination : inputAbilityValues.destination; }
        public bool IsRunning { get => isOverrideActive ? overrideAbilityValues.isRunning : inputAbilityValues.isRunning; }
        public bool IsCrouching { get => isOverrideActive ? overrideAbilityValues.isCrouching : inputAbilityValues.isCrouching; }
        public bool IsAccelerating { get => isOverrideActive ? overrideAbilityValues.isAccelerating : inputAbilityValues.isAccelerating; }
        public MovementType MovementType { get => isOverrideActive ? overrideAbilityValues.movementType : inputAbilityValues.movementType; }
        public bool IsOverriding { get => isOverrideActive; }
        #endregion

        public void SetDirection(Vector3 dir, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.SetDirection(dir);
            else overrideAbilityValues.SetDirection(dir);
        }
        public void SetDirection(Vector2 dir, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.SetDirection(dir);
            else overrideAbilityValues.SetDirection(dir);
        }
        public void SetYDirection(float y, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.SetYDirection(y);
            else overrideAbilityValues.SetYDirection(y);
        }
        public void SetDestination(Vector3 dest, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.destination = dest;
            else overrideAbilityValues.destination = dest;
        }
        public void SetRunToggle(bool isRunning, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.isRunning = isRunning;
            else overrideAbilityValues.isRunning = isRunning;
        }
        public void SetCrouchToggle(bool isCrouching, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.isCrouching = isCrouching;
            else overrideAbilityValues.isCrouching = isCrouching;
        }
        public void SetAccelerateToggle(bool isAccelerating, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.isAccelerating = isAccelerating;
            else overrideAbilityValues.isAccelerating = isAccelerating;
        }
        public void SetMovementTypeToggle(MovementType movementType, bool overrideVals = false) {
            if (!overrideVals) inputAbilityValues.movementType = movementType;
            else overrideAbilityValues.movementType = movementType;
        }

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
        public MovementType movementType;

        public void SetDirection(Vector3 dir) => direction = dir;
        public void SetDirection(Vector2 dir) => direction = new Vector3(dir.x, direction.y, dir.y);
        public void SetYDirection(float y) => direction.y = y;
    }
}