using UnityEngine;

public abstract class MovementSO : ScriptableObject
{
    [Header("Speed Settings")]
    [SerializeField] public float speedMultiplier = 1;
    public static float baseSpeed = 0.01f;
    public float speed { get => baseSpeed * speedMultiplier; }
    [SerializeField, Min(0)] public float acceleration = 1;
    [SerializeField, Min(0)] public float deceleration = 0.8f;

    [Header("Vertical Settings")]
    [SerializeField, Min(0)] public float jumpSpeed = 0.07f;
    [SerializeField] public bool isGrounded = false;
    [SerializeField, Min(0)] public float gravity = 0.2f;
    [SerializeField] public LayerMask groundLayers;
    internal abstract float FallSpeed(AbilityController absum, bool isJumping);
    public abstract void Move(AbilityController absum, Vector3 move);
}
