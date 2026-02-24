using System;
using UnityEngine;


namespace AbilitySystem {
    [Serializable]
    public abstract class AbilitySet {
        [SerializeField] internal string abilitySetName;
        [SerializeField] internal MovementAbilitySummary movement;

        public AbilitySet(string name, MovementSO movementSO) {
            abilitySetName = name;

            if (movementSO != null)
                movement = new(movementSO);
        }
        public AbilitySet(AbilitySetSO abilitySet) {
            abilitySetName = abilitySet.abilitySetName;

            if (abilitySet.movement != null) {
                movement.AbilityData = abilitySet.movement.Setup();
                movement.movementSO = abilitySet.movement;
            }
        }
    }
    public abstract class AbilitySetSO : ScriptableObject {
        [SerializeField] internal string abilitySetName;
        [SerializeField] internal MovementSO movement;
    }
}