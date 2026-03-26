using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    public interface IAbility {
        #region Inputs
        public void InputMove(Vector3 direction, bool isDashing);
        public void InputPrimaryAttack();
        public void InputPrimaryAbility();
        public void InputTransitionName(string name);
        #endregion

        #region Ability Actions
        public void ActivateIenumerator(IEnumerator enumerator);
        public void OnMoveEntity(Vector3 direction, float turnSpeed = 1);
        public void OnRotateEntity(Vector3 direction);
        public void OnEvent(string eventMessage);
        #endregion

        #region Utility Functions
        public EntityBody GetEntityBody();
        public IEnumerator RunTimelineWithEvents(TimelineEvent[] timelineInfo, DeltaEvent[] timelineEvents = null);
        public IEnumerator RunLoop(TimelineEvent[] timelineInfo);
        #endregion
    }
}