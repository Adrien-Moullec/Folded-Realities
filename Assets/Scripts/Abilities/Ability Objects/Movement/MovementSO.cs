using System;

using UnityEngine;


namespace AbilitySystem {
    public abstract class MovementSO : AbilitySO {
        public abstract void Move(EntityBody entityBody, AbilityData data, Vector3 moveInput, bool dashInput);
    }

    [Serializable]
    public class MovementAbilitySummary : AbilitySummary {
        [SerializeField] public MovementSO movementSO;
        public void Activate(EntityBody entityBody, Vector3 move, bool dashInput) => movementSO?.Move(entityBody, AbilityData, move, dashInput);
        public MovementAbilitySummary(MovementSO m) {
            movementSO = m;
            AbilityData = m.AbilityDataSetup();
        }
    }
}