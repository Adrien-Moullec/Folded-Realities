using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    /// <summary>
    /// IAbility interface works with the ability system to reference important data from the Entity Body.
    /// </summary>
    public interface IAbility {
        #region Inputs
        public void InputTransitionName(string name);
        #endregion

        #region Ability Actions
        public void OnMoveEntity(Vector3 direction, bool rotate = true);
        public void OnEntityTrack(Vector3 location);
        public void OnRotateEntity(Vector3 direction);
        public void OnAbilityEvent(string eventMessage);
        public bool IsGrounded();
        #endregion

        #region Utility Functions
        public EntityBody GetEntityBody();
        public EntityTeam GetEntityTeam { get; }
        public AbilityControllerValues GetInputValues { get; set; }
        public AbilityController GetAbilityController { get; }
        #endregion
    }
}