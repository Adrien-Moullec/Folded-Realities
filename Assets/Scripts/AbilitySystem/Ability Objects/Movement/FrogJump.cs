using UnityEngine;

namespace AbilitySystem {
    [CreateAssetMenu(fileName = "FrogMovement", menuName = MenuAssetNames.MovementAbility + "/Frog Movement", order = -1)]
    public class FrogJump : GenericPlayerMovement {

        [Header("Jump Magnitude")]
        [SerializeField] protected float jumpForward = 3f;

        protected override void OnJump(TransformingPlayerData pmd) {
            if (!(canJump && pmd.canJump)) return;

            pmd.fallSpeed = jumpSpeed * 100;
            pmd.isGrounded = false;
            pmd.remainingJumps--;

            pmd.velocity = new Vector3(pmd.velocity.x, 0, pmd.velocity.z).normalized * jumpForward;
        }
    }
}