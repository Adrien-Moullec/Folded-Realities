using UnityEngine;


namespace AbilitySystem {
    public abstract class AbilitySO : ScriptableObject {
        public abstract bool Execute(EntityBody entityBody, AbilityData data);
        public abstract bool PassEvent(EntityBody entityBody, AbilityData data);
        public abstract void FrameEvent(AbilityData data);
        public abstract AbilityData AbilityDataSetup(EntityBody entityBody);
        public abstract (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup();
        public virtual float AbilityCost() { return 0; }
    }
}