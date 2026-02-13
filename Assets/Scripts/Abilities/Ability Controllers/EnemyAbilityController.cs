using System;
using UnityEngine;
using UnityEngine.AI;


namespace AbilitySystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAbilityController : AbilityController
    {
        [Space]
        [Header("Settings")]
        [SerializeField] float playerDist = 3;

        [Space]
        [Header("Abilities")]
        [SerializeField] EnemyAbilitySetSO abilitySetSO;
        [HideInInspector] AbilitySet abilitySetList;
        private NavMeshAgent navMeshAgent;
        protected override void Awake() {
            base.Awake();
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (abilitySetSO == null) return;
            EnemyAbilitySet ab = new EnemyAbilitySet(abilitySetSO);
            abilitySetList = ab;
        }
        private void Update()
        {
            Move(PlayerManager.player.transform.position - this.transform.position, false);
        }

        public void Move(Vector3 moveInput, bool dash)
        {
            abilitySetList?.movement.movementSO.Move(
                entityBody,
                abilitySetList?.movement.AbilityData,
                Vector3.Distance(PlayerManager.player.transform.position, transform.position) > playerDist ? moveInput : Vector3.zero,
                dash);
        }

        public override void IMoveEntity(Vector3 direction)
        {
            navMeshAgent.Move(direction);
        }

        public override void IRotateEntity(Vector3 movement)
        {
            throw new NotImplementedException();
        }
    }
}