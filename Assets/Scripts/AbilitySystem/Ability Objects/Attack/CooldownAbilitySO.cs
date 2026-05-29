using UnityEngine;

using System;
using System.Collections;

namespace AbilitySystem {

    public abstract class CooldownAbilitySO : FrameAbilitySO {
        [Header("Ability Settings")]
        [Tooltip("Time for single charge of an ability.")]
        [SerializeField, Range(0.1f, 20)] protected float cooldown;
        [Tooltip("How many charges the ability has.")]
        [SerializeField, Range(1, 5)] protected int charges;
        [Tooltip("Attack area of the cooldown ability.")]
        [SerializeField] protected AreaColliderCheck attackArea;
        [Tooltip("Requires button to be let go to use again.")]
        [SerializeField] bool mustRePressToUse = false;
        [Tooltip("Allows toggle of debug view of ability.")]
        [SerializeField] protected bool debug = false;

        /// <summary>
        /// Return cooldown data class for default ability data storage.
        /// </summary>
        public override AbilityData AbilityDataSetup(EntityBody entityBody) {
            return new CooldownData(charges, cooldown);
        }
        #region Call Logic
        /// <summary>
        /// Execute ability start
        /// </summary>
        public override bool Execute(EntityBody entityBody, AbilityData data) {
            CooldownData cdd = (CooldownData)data;

            /// Checks for how ability is used depending on re-press, charges and current ability usage
            if (data.usingAbility) {
                if (data.isHoldingInput)
                    OnHold(entityBody, cdd);
                else
                    OnPressWhileUsing(entityBody, cdd);
                return false;
            } else if (entityBody.UsingAbility || cdd.currentCharges <= 0 || (mustRePressToUse && data.isHoldingInput)) {
                return false;
            }

            entityBody.iAbility.GetAbilityController.StartCoroutine(UseAbility(entityBody, cdd));
            return true;
        }
        /// <summary>
        /// Check for damage in area.
        /// </summary>
        protected void DamageAreaCheck(EntityBody entityBody, CooldownData data, int damage, EntityDamageType entityDamageType) {
            int things = attackArea.GetColliders(entityBody.bodyHolder).Invoke(data.raycastHits);

            for (int i = 0; i < things; i++)
                if (data.raycastHits[i].transform.TryGetComponent(out IHealth iHealth))
                    if (iHealth != entityBody.iHealth) {
                        iHealth.Damage(
                            new EntityDamage(
                                damage,
                                entityBody,
                                entityBody.iAbility.GetEntityTeam,
                                entityDamageType
                            )
                        );
                        Debug.Log(data.raycastHits[i].gameObject.name);
                    }
        }

        /// <summary>
        /// Initiate ability
        /// </summary>
        public override void Startup(EntityBody entityBody, AbilityData data) { }
        /// <summary>
        /// Action if no input is pressed.
        /// </summary>
        public override bool PassEvent(EntityBody entityBody, AbilityData data) {
            data.isHoldingInput = false;
            return true;
        }

        /// <summary>
        /// Action while ability is held.
        /// </summary>
        protected virtual void OnHold(EntityBody entityBody, CooldownData data) { }

        /// <summary>
        /// Action when ability button is pressed while using.
        /// </summary>
        protected virtual void OnPressWhileUsing(EntityBody entityBody, CooldownData data) { }

        /// <summary>
        /// Action that happens every frame.
        /// </summary>
        public override void FrameEvent(EntityBody entityBody, AbilityData data) {
            CooldownData cdd = (CooldownData)data;

            /// Cooldown ability
            if (cdd.currentCharges >= charges) {
                cdd.currentCharges = charges;
                cdd.cooldownDelta = 0;
            } else if (cdd.cooldownDelta > cooldown) {
                cdd.cooldownDelta = 0;
                cdd.currentCharges++;
            } else {
                cdd.cooldownDelta += Time.deltaTime;
            }
        }
        #endregion

        #region Ability Logic
        /// <summary>
        /// Use ability is the start of the ability and relies on coroutines to wait for animation states.
        /// </summary>
        protected IEnumerator UseAbility(EntityBody entityBody, CooldownData data) {

            /// On use ability data change
            data.isHoldingInput = true;
            data.usingAbility = true;
            entityBody.UsingAbility = true;
            data.currentCharges--;

            /// Wait for main ability to end
            yield return Ability(entityBody, data);

            /// On ability finished
            data.usingAbility = false;
            entityBody.UsingAbility = false;
            entityBody.MoveOverride = false;
        }

        /// <summary>
        /// Core function for ability code
        /// </summary>
        protected abstract IEnumerator Ability(EntityBody entityBody, CooldownData data);

        /// <summary>
        /// Play and wait for animation end
        /// </summary>
        protected virtual IEnumerator AttackAnimation(EntityBody entityBody, AbilityData data, AnimationType attackAnimation) {
            CooldownData cdd = (CooldownData)data;

            /// Play event by default if no animation exists
            if (entityBody.animatorManager == null) {
                AnimationEvent(new AbilityAnimationEventData(), entityBody, data);
                yield break;
            }

            /// Play animation with ability subscription
            cdd.animationPlaying = true;
            yield return entityBody.animatorManager.InitiateOneOffAnimation(
                null,
                null,
                (AbilityAnimationEventData animationData) => AnimationEvent(animationData, entityBody, data),
                () => cdd.animationPlaying = false,
                attackAnimation.ToString(),
                false
            );
            while (cdd.animationPlaying)
                yield return null;

        }

        /// <summary>
        /// Function to pass through action and animation data to time abilities with animation events.
        /// </summary>
        public abstract void AnimationEvent(AbilityAnimationEventData animationEvent, EntityBody entityBody, AbilityData animationType);
        #endregion
    }

    /// <summary>
    /// Main cooldown ability summary to hold pressed abilities and their data
    /// </summary>
    [Serializable]
    public class CooldownAbilitySummary : FrameAbilitySummary {
        [Tooltip("Cooldown ability object.")]
        [SerializeField] public CooldownAbilitySO abilitySO;

        /// <summary>
        /// Ability or non-ability depending on press input
        /// </summary>
        public override void Activate(EntityBody entityBody, bool abilityPressed) {
            if (abilityPressed) abilitySO?.Execute(entityBody, AbilityData);
            else abilitySO?.PassEvent(entityBody, AbilityData);
        }
        /// <summary>
        /// Starting function to initiate ability settings
        /// </summary>
        public override void StartUp(EntityBody entityBody) =>
            abilitySO?.Startup(entityBody, AbilityData);

        /// <summary>
        /// Event happening every frame
        /// </summary>
        public override void FrameEvent(EntityBody entityBody) =>
            abilitySO?.FrameEvent(entityBody, AbilityData);

        /// <summary>
        /// OnDrawGizmos option for ability to be used in character controller
        /// </summary>
        public override void OnDrawGizmos(EntityBody entityBody) =>
            abilitySO?.GizmoEvent(entityBody);

        /// <summary>
        /// Setup ability data on initialize
        /// </summary>
        public CooldownAbilitySummary(CooldownAbilitySO m, EntityBody eb) {
            abilitySO = m;
            AbilityData = m.AbilityDataSetup(eb);
        }
    }

    /// <summary>
    /// Base cooldown data variables
    /// </summary>
    public class CooldownData : AbilityData {
        [Tooltip("Current time to ability refreshing a charge.")]
        public float cooldownDelta;
        [Tooltip("Current charges the ability has.")]
        public int currentCharges;
        [Tooltip("Collider hits from CheckAreaCollider in CooldownAbility script.")]
        public Collider[] raycastHits;
        [Tooltip("Keep track of whether animation is currently playing.")]
        public bool animationPlaying = false;

        /// <summary>
        /// Initialize cooldown data
        /// </summary>
        public CooldownData(int charges, float cooldown) {
            currentCharges = charges;
            cooldownDelta = cooldown;
            raycastHits = new Collider[10];
        }
    }
}