using System;
using System.Collections;
using UnityEngine;


namespace AbilitySystem
{
    [Serializable]
    public abstract class AbilityData
    {
        [HideInInspector] internal float cooldownDelta;
        [HideInInspector] internal int currentCharges;
        [HideInInspector] internal bool isRecharging = false;
    }
}