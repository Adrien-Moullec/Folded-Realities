using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Rotate Towards Player", menuName = MenuAssetNames.MovementAbility + "/Rotate Towards Player", order = -1)]
    public class RotateTowardsPlayerSO : MovementSO {
        public override AbilityData AbilityDataSetup(EntityBody eb) => null;
        public override bool Execute(EntityBody eb, AbilityData data) {
            if (eb.iAbility.GetInputValues.Direction == Vector3.zero) return false;
            Vector3 dir = eb.iAbility.GetInputValues.Direction;
            dir.y = 0;
            dir.Normalize();
            eb.bodyHolder.transform.forward = dir;
            return true;
        }

        public override bool NormalMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }

        public override bool ChargeMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }

        public override bool AutoTrackMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }

        public override bool FlightMovement(EntityBody entityBody, AbilityData data, AbilityControllerValues inpVals) {
            throw new System.NotImplementedException();
        }

        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            throw new System.NotImplementedException();
        }
    }
}