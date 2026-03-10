using UnityEngine;


namespace AbilitySystem {
    public abstract class AbilitySO : ScriptableObject {
        public abstract AbilityData AbilityDataSetup();
        public abstract (AbilityAnimation, WrapMode)[] AbilityAnimationsSetup();
        public virtual float AbilityCost() { return 0; }
    }
}