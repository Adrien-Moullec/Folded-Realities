using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AbilityController {

    [Header("Body Components")]
    [SerializeField] public EntityBody entity;

    [Space]
    [Header("Abilities")]
    [SerializeField] List<AbilitySummary> AbilityList;
    internal AbilitySummary Ability;

    #region Private Variables
    [HideInInspector] internal Vector3 currentDirection;
    [HideInInspector] internal float fallSpeed = 0;
    [HideInInspector] internal float decelerationDelta = 0;
    [HideInInspector] internal bool isGrounded = false;
    #endregion

    public void Setup(GameObject original)
    {
        entity.controller = original.GetComponent<CharacterController>();
        if (AbilityList.Count > 1)
            Ability = AbilityList[0];
    }
    public void SetAbility(string name) {
        if (AbilityList.Any(x => x.abilitySetName == name))
            Ability = AbilityList.First(x => x.abilitySetName == name);
        else Debug.LogWarning("No ability to set");
    }

    #region Ability Functions
    public void Move(Vector3 moveInput)
        => Ability.movement.Move(this, moveInput);
    #endregion
}


[CreateAssetMenu(fileName = "Ability Summary", menuName = "Origami/Ability Summary/Player Movement", order = 0)]
public class AbilitySummary : ScriptableObject
{
    [SerializeField] internal string abilitySetName;
    [SerializeField] internal MovementSO movement;
    [SerializeField] internal DashSO dash;
    [SerializeField] internal FirstAttackSO firstAttack;
    [SerializeField] internal PrimaryAbilitySO primaryAbility;
}

[Serializable]
public class EntityBody
{
    public GameObject body;
    public GameObject feet;
    public CharacterController controller;
}