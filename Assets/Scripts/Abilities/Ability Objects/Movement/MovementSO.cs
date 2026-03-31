using System;

using UnityEngine;


namespace AbilitySystem {
    public abstract class MovementSO : AbilitySO {
        public override bool Execute(EntityBody entityBody, AbilityData data) {
            switch (entityBody.iAbility.GetInputValues.movementType) {
                case MovementType.Normal: return NormalMovement(entityBody, data);
                case MovementType.Charge: return ChargeMovement(entityBody, data);
                case MovementType.AutoTrack: return NormalMovement(entityBody, data);
                default: Debug.LogError("No correct movement types"); return false;
            }
        }

        public abstract bool NormalMovement(EntityBody entityBody, AbilityData data);
        public abstract bool ChargeMovement(EntityBody entityBody, AbilityData data);
        public abstract bool AutoTrackMovement(EntityBody entityBody, AbilityData data);
    }

    public enum MovementType {
        Normal,
        Charge,
        AutoTrack,
    }

    [Serializable]
    public class MovementAbilitySummary : AbilitySummary {
        [SerializeField] public MovementSO movementSO;
        public override void Activate(EntityBody entityBody) =>
            movementSO?.Execute(entityBody, AbilityData);

        public MovementAbilitySummary(MovementSO m, EntityBody eb) {
            movementSO = m;
            AbilityData = m.AbilityDataSetup(eb);
        }
    }
}