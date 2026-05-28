using System;
using System.Collections;

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

        public object EnemyAbilitySetSOmovement { get; private set; }
        public override void OnEnable() {
            navMeshAgent.enabled = true;
        }
        public override void OnDisable() {
            try {
                navMeshAgent.isStopped = true;
            } catch { Debug.LogError("Issue Stoppiung agent"); }
            navMeshAgent.enabled = false;
        }
        public override bool IsGrounded() => true;
        protected override void Awake() {
            base.Awake();
            if (!navMeshAgent.isOnNavMesh)
                if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas)) {
                    navMeshAgent.enabled = false;
                    navMeshAgent.Warp(hit.position);
                    navMeshAgent.enabled = true;
                }
            entityBody.iAbility = this;
            entityBody.iHealth = this;
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (abilitySetSO == null) return;
            abilitySet = new EnemyAbilitySet(abilitySetSO, entityBody);

            if (abilitySet.movement?.movementSO != null)
                frameEvents += () => { abilitySet?.movement?.FrameEvent(entityBody); };
            if (abilitySet?.attack?.abilitySO != null)
                frameEvents += () => { abilitySet?.attack?.FrameEvent(entityBody); };
        }
        protected override void Update() {
            base.Update();
            if (!navMeshAgent.enabled) return;
            //abilitySet?.movement?.Activate(entityBody, true);
            abilitySet?.attack?.Activate(entityBody, GetInputValues.isPrimaryAbility);
            AnimateAbility();
        }

        public override void OnMoveEntity(Vector3 direction, bool rotate = true) {
            if (navMeshAgent.isOnNavMesh) navMeshAgent.SetDestination(direction);
            direction.y = 0;
            //if (direction != Vector3.zero && rotate) entityBody.bodyHolder.transform.forward = direction;
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
            abilitySetSO.movement?.GizmoEvent(entityBody);
            abilitySetSO.attack.GizmoEvent(entityBody);
        }

        public override void Die() {
            navMeshAgent.enabled = false;
            StartCoroutine(OnDie());
        }
        IEnumerator OnDie() {
            bool isFin = false;
            yield return GetEntityBody().animatorManager.InitiateOneOffAnimation(
                null,
                null,
                null,
                () => isFin = true,
                AnimationType.Death.ToString(),
                true,
                0.2f
            );
            while (!isFin) yield return null;

            //StartCoroutine(PlayerDeath(entityBody.animatorManager, () => Destroy(gameObject)));
            Destroy(gameObject);
        }
        protected void AnimateAbility() {
            float delta = Mathf.Clamp01(navMeshAgent.velocity.magnitude / navMeshAgent.speed);

            entityBody.
            animatorManager?.
            SetMovement(
                delta,
                0,
                true
            );
        }
    }
}