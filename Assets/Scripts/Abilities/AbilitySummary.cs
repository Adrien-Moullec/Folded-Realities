using UnityEngine;

[CreateAssetMenu(fileName = "Ability Summary", menuName = "Origami/Ability Summary/Player Movement", order = 0)]
public class AbilitySummary : ScriptableObject
{
    [SerializeField] internal string abilitySetName;
    [SerializeField] internal MovementSO movement;
    [SerializeField] internal DashSO dash;
    [SerializeField] internal FirstAttackSO firstAttack;
    [SerializeField] internal PrimaryAbilitySO primaryAbility;
}