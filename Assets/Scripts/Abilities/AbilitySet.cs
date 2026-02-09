using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilitySet
{
    [SerializeField] internal string abilitySetName;
    [SerializeField] internal MovementAbilitySummary movement = new();
    [SerializeField] internal ActivatedAbilitySummary light = new();
    [SerializeField] internal ActivatedAbilitySummary heavy = new();
    [SerializeField] internal ActivatedAbilitySummary primary = new();
    
    public AbilitySet(AbilitySetSO abilitySet)
    {
        abilitySetName = abilitySet.abilitySetName;

        if (abilitySet.movement != null) {
            movement.AbilityData = abilitySet.movement.Setup();
            movement.movementSO = abilitySet.movement;
        }

        if (abilitySet.light != null) {
            light.AbilityData = abilitySet.light.Setup();
            light.abilitySO = abilitySet.light;
        }

        if (abilitySet.heavy != null) {
            heavy.AbilityData = abilitySet.heavy.Setup();
            heavy.abilitySO = abilitySet.heavy;
        }

        if (abilitySet.primary != null) {
            primary.AbilityData = abilitySet.primary.Setup();
            primary.abilitySO = abilitySet.primary;
        }
    }
}