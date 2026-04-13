using System;

using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Health", menuName = MenuAssetNames.Health)]
    public abstract class HealthSO : AbilitySO { // : AbilitySO
        [SerializeField] public int Defense;
    }

    /*[Serializable]
    public class HealthSummary : AbilitySummary {
        [SerializeField] public HealthSO healthSO;

        public HealthSummary(HealthSO m) {
            healthSO = m;
        }
    }*/
}