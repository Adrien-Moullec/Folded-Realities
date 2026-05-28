using System;
using System.Collections;
using System.Linq;

using UnityEngine;

namespace AbilitySystem {

    /// <summary>
    /// Base controller used for the Ability System that manages health and ability interpretation
    /// </summary>
    public abstract class AbilityController : MonoBehaviour, IAbility, IHealth {
        [Header("Team")]
        [Tooltip("The 'team' the entity is on.")]
        [SerializeField] public EntityTeam entityTeam;
        public EntityTeam GetEntityTeam => entityTeam;

        [Tooltip("Max entity health.")]
        public int MaxHealth = 100;
        [Tooltip("Current entity health.")]
        public int CurrentHealth;

        [Tooltip("Get the input values of an entity.")]
        public AbilityControllerValues GetInputValues { get; set; } = new();
        [Tooltip("Frame events delegate template for abilities to be stored and called.")]
        protected delegate void FrameEvents();
        [Tooltip("Ability frame events.")]
        protected FrameEvents frameEvents;
        [Tooltip("Get this controller.")]
        public AbilityController GetAbilityController { get => this; }

        [Tooltip("Set the active state of this controller.")]
        public bool isControllerActive = true;
        float hitFrameTime = 1;

        /// <summary>
        /// Health setup
        /// </summary>
        protected virtual void Awake() {
            CurrentHealth = MaxHealth;
        }

        /// <summary>
        /// Do frame events for each ability every frame
        /// </summary>
        protected virtual void Update() {
            if (!isControllerActive) return;
            frameEvents?.Invoke();
        }

        #region Movement Interface
        /// <summary>
        /// Enable controller
        /// </summary>
        public virtual void OnEnable() {
            isControllerActive = true;
        }
        /// <summary>
        /// Disable controller
        /// </summary>
        public virtual void OnDisable() {
            isControllerActive = false;
        }
        /// <summary>
        /// Rotate the entity to face a direction
        /// </summary>
        public abstract void OnRotateEntity(Vector3 movement);
        /// <summary>
        /// Move the entity towards a direction
        /// </summary>
        public abstract void OnMoveEntity(Vector3 direction, bool rotate = true);
        /// <summary>
        /// Set a location for the entity to travel to
        /// </summary>
        public abstract void OnEntityTrack(Vector3 location);
        #endregion

        #region Utility
        /// <summary>
        /// Return current body information of the entity
        /// </summary>
        public abstract EntityBody GetEntityBody();
        /// <summary>
        /// Generic string event for particular individual entity needs
        /// </summary>
        public virtual void OnAbilityEvent(string eventMessage) { }
        #endregion

        #region Health
        /// <summary>
        /// Receive damage and check for team information
        /// </summary>
        /// <param name="damage"></param>
        public virtual void Damage(EntityDamage damage) {
            if (EntityTeamFunctions.HasCommonTeam(entityTeam, damage.damagingTeam)) return;

            /// check for health SO otherwise do default damage
            if (GetEntityBody().abilitySet?.healthSettings != null)
                GetEntityBody().abilitySet?.healthSettings.DamageAmount(GetEntityBody(), ref CurrentHealth, ref MaxHealth, damage);
            else
                HealthSO.DefaultDamage(GetEntityBody(), ref CurrentHealth, ref MaxHealth, damage);
            StartCoroutine(OnHitFrames());

            /// Check for death
            if (CurrentHealth <= 0) Die();
            foreach (var n in GetEntityBody().entityShader.Select(x => x.material)) {
                n.SetFloat("_Health01", (float)CurrentHealth / MaxHealth);
            }
        }

        /// <summary>
        /// Entity damage process
        /// </summary>
        public virtual IEnumerator OnHitFrames() {
            float time = 0;
            while (time < hitFrameTime) {
                time += Time.deltaTime;
                foreach (var n in GetEntityBody().entityShader)
                    n.material.SetFloat("_DamageFlash01", Mathf.Abs(Mathf.Sin(time * 8 / hitFrameTime)));
                yield return null;
            }
            Debug.Log("END");
            foreach (var n in GetEntityBody().entityShader)
                n.material.SetFloat("_DamageFlash01", 0);
        }

        /// <summary>
        /// Heal entity by an amount
        /// </summary>
        public virtual void Heal(EntityDamage heal) {
            if (GetEntityBody().abilitySet?.healthSettings != null)
                GetEntityBody().abilitySet?.healthSettings.HealAmount(GetEntityBody(), ref CurrentHealth, ref MaxHealth, heal);
            else
                HealthSO.DefaultHeal(GetEntityBody(), ref CurrentHealth, ref MaxHealth, heal);
            foreach (var n in GetEntityBody().entityShader.Select(x => x.material))
                n.SetFloat("_Health01", (float)CurrentHealth / MaxHealth);
        }

        /// <summary>
        /// Death process
        /// </summary>
        public abstract void Die();
        /// <summary>
        /// Function for entities that transform
        /// </summary>
        /// <param name="name"> Name of target transformation </param>
        public virtual void InputTransitionName(string name) { }
        #endregion

        /// <summary>
        /// Gizmo draw
        /// </summary>
        public abstract void OnDrawGizmos();

        /// <summary>
        /// Check for ground status
        /// </summary>
        /// <returns> returns true on ground </returns>
        public abstract bool IsGrounded();

        /// <summary>
        /// Set the max health of the entity
        /// </summary>
        public virtual void SetMaxHealth() {
            CurrentHealth = MaxHealth;
            foreach (var n in GetEntityBody().entityShader)
                n.material.SetFloat("_Health01", 1);
        }
    }
}