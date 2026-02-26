using System;
using UnityEngine;
using UnityEngine.AI;


namespace AbilitySystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SingleAbilityController : AbilityController
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
            InputMove(PlayerManager.player.transform.position - transform.position, false);
        }
        
        public override void OnMoveEntity(Vector3 direction)
        {
            navMeshAgent.Move(direction);
        }

        public override void OnRotateEntity(Vector3 movement)
        {
            throw new NotImplementedException();
        }

        public override void InputMove(Vector3 direction, bool isDashing)
        {
            abilitySetList?.movement.movementSO.Move(
                entityBody,
                abilitySetList?.movement.AbilityData,
                Vector3.Distance(PlayerManager.player.transform.position, transform.position) > playerDist ? direction : Vector3.zero,
                isDashing);
        }

        public override void InputPrimaryAttack()
        {
            throw new NotImplementedException();
        }
        public override void InputPrimaryAbility()
        {
            throw new NotImplementedException();
        }

        internal override void SetupAnimations()
        {
            throw new NotImplementedException();
        }
    }
}