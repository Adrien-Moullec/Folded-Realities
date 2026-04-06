using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    public interface IAbility {
        #region Inputs
        public void InputTransitionName(string name);
        #endregion

        #region Ability Actions
        public void OnMoveEntity(Vector3 direction);
        public void OnRotateEntity(Vector3 direction);
        public void OnAbilityEvent(string eventMessage);
        #endregion

        #region Utility Functions
        public EntityBody GetEntityBody();
        public EntityTeam GetEntityTeam { get; }
        public AbilityInputValues GetInputValues { get; set; }
        public AbilityController GetAbilityController { get; }
        #endregion
    }
}