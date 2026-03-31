using System;

using UnityEngine;


namespace AbilitySystem {
    [Serializable]
    public abstract class AbilitySummary {
        public AbilityData AbilityData;
        public abstract void Activate(EntityBody entityBody);
    }
}