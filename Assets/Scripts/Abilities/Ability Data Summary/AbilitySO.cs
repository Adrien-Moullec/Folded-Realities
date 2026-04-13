using System;

using UnityEngine;


namespace AbilitySystem {
    public abstract class AbilitySO : ScriptableObject {

        #region Call Logic
        public abstract bool Execute(EntityBody entityBody, AbilityData data);
        public abstract bool PassEvent(EntityBody entityBody, AbilityData data);
        public abstract void FrameEvent(AbilityData data);
        #endregion

        #region Data Setup
        public abstract AbilityData AbilityDataSetup(EntityBody entityBody);
        public abstract void Startup(EntityBody entityBody, AbilityData data);
        public virtual float AbilityCost() { return 0; }
        #endregion

        #region Abilities
        #endregion
    }
    [Serializable]
    public abstract class AbilitySummary {
        public AbilityData AbilityData;
        public abstract void Activate(EntityBody entityBody, bool AbilityPressed);
        public abstract void StartUp(EntityBody entityBody);
        public abstract void FrameEvent();
    }
}