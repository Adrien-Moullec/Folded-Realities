using System;

using UnityEngine;


namespace AbilitySystem {
    public abstract class MovementSO : AbilitySO {
        public override bool Execute(EntityBody entityBody, AbilityData data) {
            AbilityInputValues inpVals = entityBody.iAbility.GetInputValues;
            switch (entityBody.iAbility.GetInputValues.movementType) {
                case MovementType.Normal: return NormalMovement(entityBody, data, inpVals);
                case MovementType.Charge: return ChargeMovement(entityBody, data, inpVals);
                case MovementType.AutoTrack: return NormalMovement(entityBody, data, inpVals);
                case MovementType.Flight: return FlightMovement(entityBody, data, inpVals);
                case MovementType.None: return false;
                default: Debug.LogError("No correct movement types"); return false;
            }
        }
        public override void FrameEvent(AbilityData data) { }

        public abstract bool NormalMovement(EntityBody entityBody, AbilityData data, AbilityInputValues inpVals);
        public abstract bool ChargeMovement(EntityBody entityBody, AbilityData data, AbilityInputValues inpVals);
        public abstract bool AutoTrackMovement(EntityBody entityBody, AbilityData data, AbilityInputValues inpVals);
        public abstract bool FlightMovement(EntityBody entityBody, AbilityData data, AbilityInputValues inpVals);
    }

    public enum MovementType {
        Normal,
        Charge,
        AutoTrack,
        Flight,
        None
    }

    [Serializable]
    public class MovementAbilitySummary : AbilitySummary {
        [SerializeField] public MovementSO movementSO;
        public override void Activate(EntityBody entityBody, bool isPressed) =>
            movementSO?.Execute(entityBody, AbilityData);

        public override void FrameEvent() =>
            movementSO?.FrameEvent(AbilityData);

        public MovementAbilitySummary(MovementSO m, EntityBody eb) {
            movementSO = m;
            AbilityData = m.AbilityDataSetup(eb);
        }
    }
}