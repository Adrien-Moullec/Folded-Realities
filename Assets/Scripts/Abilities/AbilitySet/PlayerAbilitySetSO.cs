using System;
using UnityEngine;



[CreateAssetMenu(fileName = "Ability Set", menuName = "Origami/Ability Sets/Player Ability Set", order = 0)]
public class PlayerAbilitySetSO : AbilitySetSO
{
    [SerializeField] internal ActivatedAbilitySO light;
    [SerializeField] internal ActivatedAbilitySO heavy;
    [SerializeField] internal ActivatedAbilitySO primary;
}

[Serializable]
public class PlayerAbilitySet : AbilitySet
{    
    [SerializeField] internal ActivatedAbilitySummary light = new();
    [SerializeField] internal ActivatedAbilitySummary heavy = new();
    [SerializeField] internal ActivatedAbilitySummary primary = new();

    public PlayerAbilitySet(PlayerAbilitySetSO abilitySet) : base(abilitySet.name, abilitySet.movement)
    {
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