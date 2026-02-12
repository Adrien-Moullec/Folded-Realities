using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAbilityController : AbilityController
{

    [Space]
    [Header("Abilities")]
    [SerializeField] EnemyAbilitySetSO abilitySetSO;
    [HideInInspector] AbilitySet abilitySetList;
    private NavMeshAgent navMeshAgent;
    public override void Setup()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (abilitySetSO == null) return;
        EnemyAbilitySet ab = new EnemyAbilitySet(abilitySetSO);
        abilitySetList = ab;
    }
    private void Update() {
        Move(PlayerManager.player.transform.position - this.transform.position, false);
    }

    public void Move(Vector3 moveInput, bool dash) => abilitySetList?.movement.movementSO.Move(entityBody, abilitySetList?.movement.AbilityData, moveInput, dash);

    public override void IMoveEntity(Vector3 direction)
    {
        navMeshAgent.Move(direction);
    }
}