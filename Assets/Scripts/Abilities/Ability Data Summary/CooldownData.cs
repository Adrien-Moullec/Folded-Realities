using AbilitySystem;
using UnityEngine;

//Come back and add the max charges and cooldown?? Might interfere with fact that IENumerator can only happen in monobehaviour
public class CooldownData : AbilityData
{    
    [HideInInspector] internal float cooldownDelta;
    [HideInInspector] internal int currentCharges;
    [HideInInspector] internal bool isRecharging = false;
    [HideInInspector] internal bool isUsing = false;
    public CooldownData() {}
    public CooldownData(int charges, float cooldown)
    {
        currentCharges = charges;
        cooldownDelta = cooldown;
    }
}