using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerAbilityController : AbilityController
{

    [Space]
    [Header("Abilities")]
    [SerializeField] List<PlayerAbilitySetSO> abilitySetSO;
    [HideInInspector] List<AbilitySet> abilitySetList = new();
    [HideInInspector] internal AbilitySet currentAbilitySet;
    private CharacterController characterController;

    public override void Setup()
    {
        characterController = GetComponent<CharacterController>();

        if (abilitySetSO.Count < 1)
            return;
        foreach (var n in abilitySetSO) {
            if (n == null)
                continue;
            PlayerAbilitySet ab = new PlayerAbilitySet(n);
            abilitySetList.Add(ab);
        }
        currentAbilitySet = abilitySetList[0];
    }

    public override void IMoveEntity(Vector3 direction)
    {
        characterController.Move(direction);
    }


    public void SetAbility(string name)
    {
        if (abilitySetList.Any(x => x.abilitySetName == name))
            currentAbilitySet = abilitySetList.First(x => x.abilitySetName == name);
        else
            Debug.LogWarning("No ability to set");
    }

    #region Ability Functions
    public void Move(Vector3 moveInput, bool dash) => currentAbilitySet?.movement.movementSO.Move(entityBody, currentAbilitySet?.movement.AbilityData, moveInput, dash);

    public override void IRotateEntity(Vector3 movement)
    {
        throw new NotImplementedException();
    }
    #endregion
}