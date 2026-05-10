using System;

using UnityEngine;


namespace AbilitySystem {
    public abstract class AbilitySO : ScriptableObject {

        #region Call Logic
        public virtual void GizmoEvent(EntityBody entityBody) { }
        #endregion

        #region Data Setup
        public virtual float AbilityCost() { return 0; }
        #endregion

        #region Abilities
        #endregion
    }
    [Serializable]
    public abstract class AbilitySummary {
        public AbilityData AbilityData;
        public abstract void OnDrawGizmos(EntityBody entityBody);
    }
}