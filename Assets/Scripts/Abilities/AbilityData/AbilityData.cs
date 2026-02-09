using System;
using UnityEngine;

[Serializable]
public abstract class AbilityData
{
    [HideInInspector] internal float cooldownDelta;
    [HideInInspector] internal int currentCharges;
}