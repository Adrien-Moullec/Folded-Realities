using System.Collections;

using UnityEngine;

namespace AbilitySystem {
    public interface IAbility {
        #region Inputs
        public void InputTransitionName(string name);
        #endregion

        #region Ability Actions
        public void ActivateIenumerator(IEnumerator enumerator);
        public void OnMoveEntity(Vector3 direction);
        public void OnRotateEntity(Vector3 direction);
        public void OnAbilityEvent(string eventMessage);
        #endregion

        #region Utility Functions
        public EntityBody GetEntityBody();
        public EntityTeam GetEntityTeam { get; }
        public IEnumerator RunTimelineWithEvents(TimelineEvent[] timelineInfo, DeltaEvent[] timelineEvents = null);
        public IEnumerator RunLoop(TimelineEvent[] timelineInfo);
        public AbilityInputValues GetInputValues { get; set; }
        #endregion
    }
}