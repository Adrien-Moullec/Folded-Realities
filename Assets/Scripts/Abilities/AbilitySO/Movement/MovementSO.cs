using System;
using UnityEngine;

public abstract class MovementSO : BaseAbility
{

    internal abstract void Move(EntityBody entityBody, AbilityData data, Vector3 moveInput, bool dashInput);
}


[Serializable]
public class MovementAbilitySummary : AbilitySummary
{
    [SerializeField] internal MovementSO movementSO;
    internal void Activate(PlayerAbilityController absum, Vector3 move, bool dashInput)
    {
        movementSO.Move(absum.entityBody, AbilityData, move, dashInput);
    }
}