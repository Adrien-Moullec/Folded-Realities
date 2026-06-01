using System;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;

namespace AbilitySystem {

    /// <summary>
    /// Base enemy controller for the ability system for abilities, navmesh control and health
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class SingleSetBaseEnemyController : AbilityController {
        [Space]
        [Header("Settings")]
        [Tooltip("Enemy sets for abilities.")]
        [SerializeField] EnemyAbilitySetSO abilitySetSO;
        [Tooltip("Enemy Entity information.")]
        [SerializeField] protected EntityBody entityBody;
        [Tooltip("Ability set list after interpreting the scriptable object.")]
        [HideInInspector] protected EnemyAbilitySet abilitySet;
        [Tooltip("NavMesh reference")]
        [SerializeField] private NavMeshAgent navMeshAgent;

        /// <summary>
        /// Enable navmeshagent
        /// </summary>
        public override void OnEnable() {
            navMeshAgent.enabled = true;
        }

        /// <summary>
        /// Disable navmeshagent
        /// </summary>
        public override void OnDisable() {
            try {
                navMeshAgent.isStopped = true;
            } catch { Debug.LogError("Issue Stoppiung agent"); }
            navMeshAgent.enabled = false;
        }

        /// <summary>
        /// Returns grounded state for IAbility interface
        /// </summary>
        public override bool IsGrounded() => true;

        /// <summary>
        /// Enemy setup
        /// </summary>
        protected override void Awake() {
            base.Awake();

            /// Move enemy to navmesh
            if (!navMeshAgent.isOnNavMesh)
                if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas)) {
                    navMeshAgent.enabled = false;
                    navMeshAgent.Warp(hit.position);
                    navMeshAgent.enabled = true;
                }

            /// Set interfaces and components for ability system
            entityBody.iAbility = this;
            entityBody.iHealth = this;
            navMeshAgent = GetComponent<NavMeshAgent>();

            /// Setup abilities
            if (abilitySetSO == null) return;
            abilitySet = new EnemyAbilitySet(abilitySetSO, entityBody);

            /// Setup frame events for abilities
            if (abilitySet.movement?.movementSO != null)
                frameEvents += () => { abilitySet?.movement?.FrameEvent(entityBody); };
            if (abilitySet?.attack?.abilitySO != null)
                frameEvents += () => { abilitySet?.attack?.FrameEvent(entityBody); };
            if (abilitySet?.attack2?.abilitySO != null)
                frameEvents += () => { abilitySet?.attack2?.FrameEvent(entityBody); };
        }

        /// <summary>
        /// Check input values for ability use every frame
        /// </summary>
        protected override void Update() {
            base.Update();
            if (!navMeshAgent.enabled) return;
            //abilitySet?.movement?.Activate(entityBody, true);
            abilitySet?.attack?.Activate(entityBody, GetInputValues.isPrimaryAbility);
            abilitySet?.attack2?.Activate(entityBody, GetInputValues.isSecondaryAbility);
            AnimateAbility();
        }

        #region Movement options
        /// <summary>
        /// Move navmesh agent in a direction
        /// </summary>
        public override void OnMoveEntity(Vector3 direction, bool rotate = true) {
            if (navMeshAgent.isOnNavMesh) navMeshAgent.SetDestination(direction);
            direction.y = 0;
        }

        /// <summary>
        /// Rotate entity towards a direction
        /// </summary>
        public override void OnRotateEntity(Vector3 direction) {
            direction.y = 0;
            if (direction != Vector3.zero) entityBody.bodyHolder.transform.forward = direction;
        }
        /// <summary>
        /// Set travel-to location
        /// </summary>
        public override void OnEntityTrack(Vector3 location) {
            navMeshAgent.destination = location;
        }
        /// <summary>
        /// Get entity body information using IAbility
        /// </summary>
        public override EntityBody GetEntityBody() => entityBody;

        /// <summary>
        /// Draw Gizmos from attack and movement options
        /// </summary>
        public override void OnDrawGizmos() {

            if (entityBody == null)
                return;

            if (abilitySetSO == null)
                return;

            if (abilitySetSO.movement != null)
                abilitySetSO.movement.GizmoEvent(entityBody);

            if (abilitySetSO.attack != null)
                abilitySetSO.attack.GizmoEvent(entityBody);

            if (abilitySetSO.attack2 != null)
                abilitySetSO.attack2.GizmoEvent(entityBody);
        }
        #endregion

        #region Health
        /// <summary>
        /// Disable navmesh and start death sequence
        /// </summary>
        public override void Die() {
            navMeshAgent.enabled = false;
            StartCoroutine(OnDie());
        }

        /// <summary>
        /// Perform death sequence on health 0
        /// </summary>
        /// <returns></returns>
        IEnumerator OnDie() {
            bool isFin = false;
            yield return GetEntityBody().animatorManager.InitiateOneOffAnimation(
                null,
                null,
                null,
                () => Destroy(gameObject),
                AnimationType.Death.ToString(),
                true,
                0.2f
            );
            while (!isFin) yield return null;

        }
        #endregion

        /// <summary>
        /// Set animator manager movement blend tree from navmesh speed
        /// </summary>
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