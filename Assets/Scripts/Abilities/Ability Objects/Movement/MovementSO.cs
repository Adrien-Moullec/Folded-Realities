using System;

using UnityEngine;


namespace AbilitySystem {
    public abstract class MovementSO : AbilitySO {
        public override bool Execute(EntityBody entityBody, AbilityData data) {
            AbilityControllerValues inpVals = entityBody.iAbility.GetInputValues;

            switch (inpVals.MovementType) {
                case MovementType.Normal: return NormalMovement(entityBody, data, inpVals);
                case MovementType.Charge: return ChargeMovement(entityBody, data, inpVals);
                case MovementType.AutoTrack: return AutoTrackMovement(entityBody, data, inpVals);
                case MovementType.Flight: return FlightMovement(entityBody, data, inpVals);
                case MovementType.None: return false;
                default: Debug.LogError("No correct movement types"); return false;
            }
        }
        public override void Startup(EntityBody entityBody, AbilityData data) {
        }
        public override void FrameEvent(EntityBody entityBody, AbilityData data) { }
        public abstract bool NormalMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals);
        public abstract bool ChargeMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals);
        public abstract bool AutoTrackMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals);
        public abstract bool FlightMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals);
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

        public override void FrameEvent(EntityBody entityBody) =>
            movementSO?.FrameEvent(entityBody, AbilityData);

        public override void StartUp(EntityBody entityBody) =>
            movementSO?.Startup(entityBody, AbilityData);
        public override void OnDrawGizmos(EntityBody entityBody) =>
            movementSO?.GizmoEvent(entityBody);
        

        public MovementAbilitySummary(MovementSO m, EntityBody eb) {
            movementSO = m;
            AbilityData = m.AbilityDataSetup(eb);
        }
    }
}