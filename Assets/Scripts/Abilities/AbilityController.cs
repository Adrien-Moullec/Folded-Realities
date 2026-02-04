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
    [SerializeField] public Transform testCube;

    #region Private Variables
    [HideInInspector] internal Vector3 currentDirection;
    [HideInInspector] internal float fallSpeed = 0;
    [HideInInspector] internal float decelerationDelta = 0;
    [HideInInspector] internal bool isGrounded = false;
    #endregion

    public void Setup(GameObject original)
    {
        entity.controller = original.GetComponent<CharacterController>();
        if (AbilityList.Count >= 1){
            Ability = AbilityList[0];
        }
    }
    public void SetAbility(string name) {
        if (AbilityList.Any(x => x.abilitySetName == name))
            Ability = AbilityList.First(x => x.abilitySetName == name);
        else Debug.LogWarning("No ability to set");
    }
    public void SetGroundedStatus(bool state) => isGrounded = state;

    #region Ability Functions
    public void Move(Vector3 moveInput) {
        Ability?.movement.Move(this, moveInput);
    }
    #endregion
}

[Serializable]
public class EntityBody
{
    public GameObject body;
    public SphereCollider feet;
    public CharacterController controller;
}