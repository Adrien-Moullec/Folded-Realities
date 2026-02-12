using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability Set", menuName = "Origami/Ability Sets/Base Enemy Ability Set", order = 0)]
public class EnemyAbilitySetSO : AbilitySetSO
{
    [SerializeField] internal ActivatedAbilitySO attack;
}

[Serializable]
public class EnemyAbilitySet : AbilitySet
{    
    [SerializeField] internal ActivatedAbilitySummary attack = new();

    public EnemyAbilitySet(EnemyAbilitySetSO abilitySet) : base(abilitySet.name, abilitySet.movement)
    {
        if (abilitySet.attack != null) {
            attack.AbilityData = abilitySet.attack.Setup();
            attack.abilitySO = abilitySet.attack;
        }
    }
}