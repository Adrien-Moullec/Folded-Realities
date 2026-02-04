using UnityEngine;

public abstract class MovementSO : ScriptableObject
{
    [Header("Speed Settings")]
    [SerializeField] internal float speedMultiplier = 1;
    internal static float baseSpeed = 0.01f;
    internal float speed { get => baseSpeed * speedMultiplier; }
    [SerializeField, Min(0)] internal float acceleration = 1;
    [SerializeField, Min(0)] internal float deceleration = 0.8f;

    [Header("Vertical Settings")]
    [SerializeField, Min(0)] internal float jumpSpeed = 0.07f;
    [SerializeField] internal bool isGrounded = false;
    [SerializeField, Min(0)] internal float gravity = 0.2f;
    [SerializeField, Min(0)] internal float maxFallSpeed = 10f;
    [SerializeField] internal LayerMask groundLayers;
    internal abstract float FallSpeed(AbilityController absum, bool isJumping);
    internal abstract void Move(AbilityController absum, Vector3 move);
}
