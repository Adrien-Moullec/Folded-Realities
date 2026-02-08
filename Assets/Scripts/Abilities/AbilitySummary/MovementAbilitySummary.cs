using UnityEngine;
using System;

[Serializable]
public class MovementAbilitySummary : AbilitySummary
{
    [SerializeField] internal MovementSO movementSO;
    internal void Activate(AbilityController absum, Vector3 move, bool dashInput)
    {
        movementSO.Move(absum, AbilityData, move, dashInput);
    }
}