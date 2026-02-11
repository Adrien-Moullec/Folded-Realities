using UnityEngine;

public abstract class AbilitySetSO : ScriptableObject
{    
    [SerializeField] internal string abilitySetName;
    [SerializeField] internal MovementSO movement;
}