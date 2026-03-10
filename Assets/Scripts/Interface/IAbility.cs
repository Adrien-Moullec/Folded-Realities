using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    public interface IAbility {
        #region Inputs
        public void InputMove(Vector3 direction, bool isDashing);
        public void InputPrimaryAttack();
        public void InputPrimaryAbility();
        #endregion

        #region Ability Actions
        public void OnActivateCooldownAbility(AbilityAnimation animInfo, (Animation component, Transform transform) modelInfo, (IEnumerator action, float delta)[] dEvents, CooldownData data, float cooldown, int maxCharges);
        public void OnMoveEntity(Vector3 direction, float turnSpeed = 1);
        public void OnRotateEntity(Vector3 direction);
        #endregion

        #region Utility Functions
        public EntityBody GetEntityBody();
        #endregion
    }
}