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
        [HideInInspector] protected AbilitySet abilitySetList;
        [HideInInspector] protected EnemyAbilitySet abilitySet;
        [SerializeField] private NavMeshAgent navMeshAgent;
        protected override void Awake() {
            base.Awake();
            entityBody.iAbility = this;
            entityBody.iHealth = this;
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (abilitySetSO == null) return;
            abilitySet = new EnemyAbilitySet(abilitySetSO, entityBody);
        }
        protected override void Update() {
            base.Update();
            Debug.Log(entityBody.animatorManager.name);
            abilitySet?.movement?.Activate(entityBody, true);
            abilitySet?.attack?.Activate(entityBody, GetInputValues.isPrimaryAbility);
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
        public override void OnEntityTrack(Vector3 location) {
            navMeshAgent.destination = location;
        }
        public override EntityBody GetEntityBody() => entityBody;
        public override void OnDrawGizmos() {
        }
    }
}