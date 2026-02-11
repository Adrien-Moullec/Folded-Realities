using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(CharacterController))]
public class PlayerAbilityController : AbilityController {

    [Header("Body Components")]
    [SerializeField] public EntityBody entity;

    [Space]
    [Header("Abilities")]
    [SerializeField] List<PlayerAbilitySetSO> abilitySetSO;
    [HideInInspector] List<AbilitySet> abilitySetList = new();
    [HideInInspector] internal AbilitySet currentAbilitySet;

    void Awake()
    {
        Setup();
        entity.controller = GetComponent<CharacterController>();
    }
    public void Setup()
    {
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
    public void SetAbility(string name) {
        if (abilitySetList.Any(x => x.abilitySetName == name))
            currentAbilitySet = abilitySetList.First(x => x.abilitySetName == name);
        else
            Debug.LogWarning("No ability to set");
    }

    #region Ability Functions
    public void Move(Vector3 moveInput, bool dash) => currentAbilitySet?.movement.movementSO.Move(this, currentAbilitySet?.movement.AbilityData, moveInput, dash);
    #endregion
}