using UnityEngine;

public abstract class BaseAbility : ScriptableObject
{
    [Header("Ability Settings")]
    [SerializeField] internal float cooldown;
    [SerializeField] internal int charges;
    public abstract AbilityData Setup();
}