using System;

using UnityEngine;

namespace AbilitySystem {
    [Serializable]
    public class AbilityInputValues {

        [Header("Movement Values")]
        [Tooltip("Current directional input for movement scripts.")]
        public Vector3 inputDirection;
        public MovementType movementType = MovementType.Normal;
        public void SetDirection(Vector3 dir) {
            if (!LockValues) inputDirection = dir;
        }
        public void SetHorizontalDirection(Vector2 dir) {
            if (!LockValues) inputDirection = new Vector3(dir.x, inputDirection.y, dir.y);
        }
        public void SetVerticalDirection(float y) {
            if (!LockValues) inputDirection = new Vector3(inputDirection.x, y, inputDirection.z);
        }
        [Tooltip("Destination value for auto-movement/enemies.")]
        public Vector3 destination = Vector3.zero;
        [Tooltip("Whether the entity is running or not.")]
        public bool isRunning = false;
        [Tooltip("Switch between accelerating and not accelerating.")]
        public bool isAccelerating = true;
        [Tooltip("Speed multiplier for outside input.")]
        public float speedMult = 1;

        [Header("Ability inputs.")]
        [Tooltip("If the primary attack button is being pressed.")]
        public bool isPrimaryAttack = false;
        [Tooltip("Locks the values.")]

        public bool LockValues = false;
    }
}