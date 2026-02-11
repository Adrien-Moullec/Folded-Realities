using System;
using UnityEngine;

public abstract class MovementSO : BaseAbility
{
    [Header("Speed Settings")]
    internal static float baseSpeed = 0.01f;
    [SerializeField] internal float speedMultiplier = 1;
    internal float speed { get => baseSpeed * speedMultiplier; }
    [SerializeField, Min(0)] internal float acceleration = 1;
    [SerializeField, Min(0)] internal float deceleration = 0.1f;

    [Header("Dash Settings")]
    [SerializeField] internal float dashSpeedMultiplier = 1.5f;
    [SerializeField] internal float dashAccelerationMultiplier = 1;

    [Header("Vertical Settings")]
    [SerializeField, Min(0)] internal float jumpSpeed = 0.07f;
    [SerializeField] internal bool isGrounded = false;
    [SerializeField, Min(0)] internal float gravity = 0.2f;
    [SerializeField, Min(0)] internal float maxFallSpeed = 10f;
    [SerializeField] internal LayerMask groundLayers;

    internal abstract float FallSpeed(PlayerAbilityController absum, AbilityData data, bool isJumping);
    internal abstract void Move(PlayerAbilityController absum, AbilityData data, Vector3 move, bool dashInput);
}


[Serializable]
public class MovementAbilitySummary : AbilitySummary
{
    [SerializeField] internal MovementSO movementSO;
    internal void Activate(PlayerAbilityController absum, Vector3 move, bool dashInput)
    {
        movementSO.Move(absum, AbilityData, move, dashInput);
    }
}