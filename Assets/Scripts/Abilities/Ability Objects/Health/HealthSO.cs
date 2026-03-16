using System;

using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Health", menuName = "")]
    public abstract class HealthSO : ScriptableObject { // : AbilitySO
        [SerializeField] public int Defense;
    }

    /*
    [Serializable]
    public class HealthSummary : AbilitySummary {
        [SerializeField] public HealthSO healthSO;

        public HealthSummary(HealthSO m) {
            healthSO = m;
        }
    }*/
}