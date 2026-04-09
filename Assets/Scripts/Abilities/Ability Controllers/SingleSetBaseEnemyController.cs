using System;

using UnityEngine;
using UnityEngine.AI;

namespace AbilitySystem {
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class SingleSetBaseEnemyController : AbilityController {
        [Space]
        [Header("Settings")]
        [SerializeField] EnemyAbilitySetSO abilitySetSO;
        [SerializeField] protected EntityBody entityBody;
        [SerializeField] float playerDist = 3;
        [HideInInspector] protected AbilitySet abilitySetList;
        private NavMeshAgent navMeshAgent;
        protected override void Awake() {
            base.Awake();
            entityBody.iAbility = this;
            entityBody.iHealth = this;
            navMeshAgent = GetComponent<NavMeshAgent>();
            entityBody.animatorManager = GetComponent<AnimatorManager>();
            if (abilitySetSO == null) return;
            EnemyAbilitySet ab = new EnemyAbilitySet(abilitySetSO, entityBody);
            abilitySetList = ab;
        }
        protected override void Update() {
            base.Update();
        }

        public override void OnMoveEntity(Vector3 direction) {
            navMeshAgent.Move(direction);
            direction.y = 0;
            if (direction != Vector3.zero) entityBody.bodyHolder.transform.forward = direction;
        }
        public override void OnRotateEntity(Vector3 direction) {
            direction.y = 0;
            if (direction != Vector3.zero) entityBody.bodyHolder.transform.forward = direction;
        }
        public override EntityBody GetEntityBody() => entityBody;
    }
}