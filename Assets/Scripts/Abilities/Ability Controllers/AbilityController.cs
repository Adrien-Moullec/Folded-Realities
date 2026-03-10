using UnityEngine;

using System;
using System.Collections;
using System.Linq;

namespace AbilitySystem {
    public abstract class AbilityController : MonoBehaviour, IAbility {
        protected virtual void Awake() {
            SetupAnimations();
        }

        internal abstract void SetupAnimations();
        #region Input Interface
        public abstract void InputMove(Vector3 direction, bool isRunning);
        public abstract void InputPrimaryAttack();
        public abstract void InputPrimaryAbility();
        #endregion

        #region Movement Interface
        public abstract void OnRotateEntity(Vector3 movement);
        public abstract void OnMoveEntity(Vector3 direction, float turnSpeed = 1);
        #endregion

        #region Ability Events Manager and Interface
        public void OnActivateCooldownAbility(AbilityAnimation abilityAnimation, Animation animation, (IEnumerator, AnimationDeltaEventInfo)[] dEvents, CooldownData data, float cooldown, int maxCharges) => StartCoroutine(ActivateCooldownAbility(abilityAnimation, animation, dEvents, data, cooldown, maxCharges));

        IEnumerator ActivateCooldownAbility(AbilityAnimation abilityAnimation, Animation animation, (IEnumerator, AnimationDeltaEventInfo)[] dEvents, CooldownData data, float cooldown, int maxCharges) {
            data.currentCharges--;
            StartCoroutine(CooldownSequence(data, cooldown, maxCharges));

            data.isUsing = true;
            yield return RunAnimationWithEvents(abilityAnimation, animation, dEvents);
            data.isUsing = false;
        }
        public IEnumerator RunAnimationWithEvents(AbilityAnimation anim, Animation animComponent, (IEnumerator, AnimationDeltaEventInfo)[] deltaEvents = null) {
            anim.Play(animComponent);
            AnimationState state = animComponent[anim.clipName];
            (IEnumerator, AnimationDeltaEventInfo)[] dEvs = deltaEvents.OrderBy(x => x.Item2.delta).ToArray();

            int counter = 0;
            if (dEvs?.Length == 0)
                goto EndOfDeltaEvents;
            yield return null;

            while (state.enabled) {
                anim.SetWeight(animComponent, anim.weightOverTime.length > 0 ? anim.weightOverTime.Evaluate(state.normalizedTime) : 1);

                if (state.normalizedTime >= dEvs[counter].Item2.delta) {
                    StartCoroutine(dEvs[counter].Item1);
                    if (dEvs.Length == ++counter) break;
                }

                yield return null;
            }

        EndOfDeltaEvents:
            while (state.enabled) yield return null;
        }
        #endregion

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

        public abstract EntityBody GetEntityBody();
    }
}