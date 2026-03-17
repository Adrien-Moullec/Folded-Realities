using UnityEngine;

using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace AbilitySystem {
    public abstract class AbilityController : MonoBehaviour, IAbility, IHealth {

        [Header("Health")]
        [Tooltip("Max health.")]
        [SerializeField, Min(1)] int maxHealth = 100;
        [Tooltip("Current entity health.")]
        protected int currentHealth = 0;

        [HideInInspector] protected bool canUseAbilities = true;

        public float CurrentHealth => throw new System.NotImplementedException();

        public float MaxHealth => throw new System.NotImplementedException();

        protected virtual void Awake() {
            currentHealth = maxHealth;
            SetupAnimations();
        }

        public abstract void SetupAnimations();
        #region Input Interface
        public virtual void InputTransitionName(string back) { }
        public virtual void InputMove(Vector3 direction, bool isRunning) {
            if (!canUseAbilities) return;
        }
        public virtual void InputPrimaryAttack() {
            if (!canUseAbilities) return;
        }
        public virtual void InputPrimaryAbility() {
            if (!canUseAbilities) return;
        }
        #endregion

        #region Movement Interface
        public abstract void OnRotateEntity(Vector3 movement);
        public abstract void OnMoveEntity(Vector3 direction, float turnSpeed = 1);
        #endregion

        #region Ability Events Manager and Interface
        public void OnActivateCooldownAbility(TimelineEvent[] timelineEvents, DeltaEvent[] dEvents, CooldownData data, float cooldown, int maxCharges) => StartCoroutine(ActivateCooldownAbility(timelineEvents, dEvents, data, cooldown, maxCharges));
        IEnumerator ActivateCooldownAbility(TimelineEvent[] timelineEvents, DeltaEvent[] dEvents, CooldownData data, float cooldown, int maxCharges) {
            data.currentCharges--;
            StartCoroutine(CooldownSequence(data, cooldown, maxCharges));

            data.isUsing = true;
            yield return RunAnimationsWithEvents(
                timelineEvents,
                dEvents
            );
            data.isUsing = false;
        }
        public IEnumerator RunAnimationsWithEvents(TimelineEvent[] timelineInfo, DeltaEvent[] timelineEvents = null) {
            float time = 0;
            float endTime = timelineInfo.Max(x => x.end);
            DeltaEvent[] dEvs = timelineEvents.OrderBy(x => x.deltaTime).ToArray();
            Dictionary<TimelineEvent, bool> isPlaying = new();

            int eventCounter = 0;

            foreach (var n in timelineInfo)
                isPlaying.Add(n, false);

            do {
                time += Time.deltaTime;

                if (dEvs?.Length > eventCounter) {

                    if (time / endTime >= dEvs[eventCounter].deltaTime) {
                        StartCoroutine(dEvs[eventCounter].action);
                        if (dEvs.Length == ++eventCounter) break;
                    }
                }
                foreach (TimelineEvent n in timelineInfo) {
                    RunAnimationCycle(
                        n,
                        time,
                        time >= n.start && time <= n.end,
                        isPlaying
                    );
                }
                yield return null;
            } while (time < endTime);

            foreach (var n in timelineInfo)
                n.anim.Stop(n.component);
        }
        private void RunAnimationCycle(TimelineEvent n, float time, bool canPlay, Dictionary<TimelineEvent, bool> isPlaying) {
            if (canPlay) {
                if (!isPlaying[n]) {
                    isPlaying[n] = true;
                    n.anim.MixTransform(n.component, n.transform);
                }
                float t = Mathf.InverseLerp(n.start, n.end, time);
                n.anim.PlayOnTimeline(
                    n.component,
                    n.reverse ? 1f - t : t
                );
            } else {
                if (isPlaying[n]) {
                    isPlaying[n] = false;
                    n.anim.Stop(n.component);
                }
            }
        }
        public static IEnumerator CooldownSequence(CooldownData data, float cooldown, int maxCharges) {
            data.isRecharging = true;
            data.cooldownDelta = cooldown;
            while (data.currentCharges < maxCharges) {
                yield return null;
                data.cooldownDelta -= Time.deltaTime;

                if (data.cooldownDelta <= 0) {
                    data.currentCharges++;
                    data.cooldownDelta = cooldown;
                }
            }
            data.isRecharging = false;
        }
        #endregion

        #region 
        public abstract EntityBody GetEntityBody();
        public virtual void OnEvent(string eventMessage) { }
        #endregion


        #region Health
        public virtual void Damage(float amount) {
            currentHealth -= (int)amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth <= 0)
                Die();
        }

        public virtual void Heal(float amount) {
            currentHealth += (int)amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        public virtual void Die() {

        }
        #endregion
    }
}