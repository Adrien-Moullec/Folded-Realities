using UnityEngine;

using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

using Unity.VisualScripting;

namespace AbilitySystem {
    public abstract class AbilityController : MonoBehaviour, IAbility {
        protected virtual void Awake() {
            SetupAnimations();
        }

        public abstract void SetupAnimations();
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
        public void OnActivateCooldownAbility((Animation component, AbilityAnimation anim, Transform transform, float start, float end)[] timelineEvents, (IEnumerator action, float delta)[] dEvents, CooldownData data, float cooldown, int maxCharges) => StartCoroutine(ActivateCooldownAbility(timelineEvents, dEvents, data, cooldown, maxCharges));
        IEnumerator ActivateCooldownAbility((Animation component, AbilityAnimation anim, Transform transform, float start, float end)[] timelineEvents, (IEnumerator action, float delta)[] dEvents, CooldownData data, float cooldown, int maxCharges) {
            data.currentCharges--;
            StartCoroutine(CooldownSequence(data, cooldown, maxCharges));

            data.isUsing = true;
            yield return RunAnimationWithEvents(
                timelineEvents,
                dEvents
            );
            data.isUsing = false;
        }

        public IEnumerator RunAnimationWithEvents((Animation component, AbilityAnimation anim, Transform transform, float start, float end)[] timelineInfo, (IEnumerator action, float delta)[] timelineEvents = null) {
            float time = 0;
            float endTime = timelineInfo.Max(x => x.end);
            (IEnumerator action, float delta)[] dEvs = timelineEvents.OrderBy(x => x.delta).ToArray();
            Dictionary<(Animation component, AbilityAnimation anim, Transform transform, float start, float end), bool> isPlaying = new();

            if (dEvs?.Length == 0)
                goto EndOfDeltaEvents;
            int eventCounter = 0;

            foreach (var n in timelineInfo)
                isPlaying.Add(n, false);

            while (time < endTime) {
                time += Time.deltaTime;
                if (time / endTime >= dEvs[eventCounter].delta) {
                    StartCoroutine(dEvs[eventCounter].Item1);
                    if (dEvs.Length == ++eventCounter) break;
                }
                foreach (var n in timelineInfo) {
                    RunAnimationCycle(
                        n,
                        time,
                        time >= n.start && time <= n.end,
                        isPlaying
                    );
                }


                yield return null;
            }

        EndOfDeltaEvents:
            while (time < endTime) {
                time += Time.deltaTime;
                foreach (var n in timelineInfo) {
                    RunAnimationCycle(
                        n,
                        time,
                        time >= n.start && time <= n.end,
                        isPlaying
                    );
                }
            }
            foreach (var n in timelineInfo)
                n.anim.Stop(n.component);
        }
        private void RunAnimationCycle((Animation component, AbilityAnimation anim, Transform transform, float start, float end) n, float time, bool canPlay, Dictionary<(Animation component, AbilityAnimation anim, Transform transform, float start, float end), bool> isPlaying) {
            if (canPlay) {
                if (!isPlaying[n]) {
                    isPlaying[n] = true;
                    n.anim.MixTransform(n.component, n.transform);
                }
                n.anim.PlayOnTimeline(n.component, n.transform, Mathf.InverseLerp(n.start, n.end, time));
            } else {
                if (isPlaying[n]) {
                    isPlaying[n] = false;
                    n.anim.Stop(n.component);
                }
            }
        }



        /*
        public IEnumerator RunAnimationWithEvents((AbilityAnimation anim, float start, float end)[] animsInfo, (Animation component, Transform transform) modelInfo, (IEnumerator action, float delta)[] deltaEvents = null) {
            animsInfo.MixTransform(modelInfo);
            AnimationState state = animsInfo.GetState(modelInfo.component);
            (IEnumerator action, float delta)[] dEvs = deltaEvents.OrderBy(x => x.delta).ToArray();

            int counter = 0;
            if (dEvs?.Length == 0)
                goto EndOfDeltaEvents;
            yield return null;

            while (state.enabled) {

                if (state.normalizedTime >= dEvs[counter].delta) {
                    StartCoroutine(dEvs[counter].Item1);
                    if (dEvs.Length == ++counter) break;
                }

                animsInfo.SetWeight(modelInfo.component, animsInfo.weightOverTime.length > 0 ? animsInfo.weightOverTime.Evaluate(state.normalizedTime) : 1);
                yield return null;
            }

        EndOfDeltaEvents:
            while (state.enabled) {
                animsInfo.SetWeight(modelInfo.component, animsInfo.weightOverTime.length > 0 ? animsInfo.weightOverTime.Evaluate(state.normalizedTime) : 1);
                yield return null;
            }
        }*/
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