using UnityEngine;


namespace AbilitySystem
{
    [CreateAssetMenu(fileName = "Rotate Towards Player", menuName = "Origami/Movement/Rotate Towards Player", order = -1)]
    public class RotateTowardsPlayerSO : MovementSO
    {
        [SerializeField] float spinSpeed = 5;
        public override AbilityData Setup() => null;
        internal override void Move(EntityBody entityBody, AbilityData data, Vector3 moveInput, bool dashInput)
        {
            if (moveInput == Vector3.zero) return;
            Vector3 dir = moveInput;
            dir.y = 0;
            dir.Normalize();
            entityBody.bodyHolder.transform.forward = dir;
        }
    }
}