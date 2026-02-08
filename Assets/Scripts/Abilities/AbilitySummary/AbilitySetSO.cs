using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Ability Set", menuName = "Origami/Abilities/Ability Set", order = 0)]
public class AbilitySetSO : ScriptableObject
{
    [SerializeField] internal string abilitySetName;
    [SerializeField] internal MovementSO movement;
    [SerializeField] internal ActivatedAbilitySO light;
    [SerializeField] internal ActivatedAbilitySO heavy;
    [SerializeField] internal ActivatedAbilitySO primary;
}