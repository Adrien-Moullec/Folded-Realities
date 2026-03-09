using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace AbilitySystem
{
    public abstract class AbilityController : MonoBehaviour, IAbility
    {
        protected virtual void Awake()
        {
            SetupAnimations();
        }

        internal abstract void SetupAnimations();
        #region Input Interface
        public abstract void InputMove(Vector3 direction, bool isRunning);
        public abstract void InputPrimaryAttack();
        public abstract void InputPrimaryAbility();
        #endregion

        #region Ability Interface
        public abstract void OnRotateEntity(Vector3 movement);
        public abstract void OnMoveEntity(Vector3 direction, float turnSpeed = 1);
        public void OnActivateCooldownAbility(AbilityAnimation abilityAnimation, Animation animationComponent, IEnumerator ability, CooldownData data, float cooldown, int maxCharges) => StartCoroutine(ActivateCooldownAbility(abilityAnimation, animationComponent, ability, data, cooldown, maxCharges));

        IEnumerator ActivateCooldownAbility(AbilityAnimation abilityAnimation, Animation animationComponent, IEnumerator ability, CooldownData data, float cooldown, int maxCharges)
        {
            data.currentCharges--;
            StartCoroutine(CooldownSequence(data, cooldown, maxCharges));

            data.isUsing = true;
            yield return RunAnimationWithEvent(abilityAnimation, animationComponent, ability);
            data.isUsing = false;
        }
        public IEnumerator RunAnimationWithEvent(AbilityAnimation abilityAnimation, Animation animationComponent, IEnumerator ability)//, Transform body)
        {

            animationComponent.Play(abilityAnimation.clipName);
            Debug.Log("Start");
            AnimationState state = animationComponent[abilityAnimation.clipName];

            //Transform[] transforms = new Transform[4];
            //animationComponent["NAME"].AddMixingTransform(transforms[0]);
            //animationComponent["NAME"].AddMixingTransform(transforms[1]);

            while (state.enabled)
            {
                animationComponent[abilityAnimation.clipName].weight = abilityAnimation.weightOverTime.Evaluate(state.normalizedTime);
                if (state.normalizedTime >= abilityAnimation.abilityEventDelta)
                {
                    Debug.Log("AbilityTIME");
                    yield return StartCoroutine(ability);
                    break;
                }
                float seconds = state.normalizedTime * state.length;
                yield return null;
            }
            while (state.enabled) yield return null;
            Debug.Log("End");
        }
        public IEnumerator RunAnimationWithEvent(AbilityAnimation abilityAnimation, Animation animationComponent, Action ability) => RunAnimationWithEvent(abilityAnimation, animationComponent, ActionToIenumerator(ability));
        private IEnumerator ActionToIenumerator(Action action)
        {
            action?.Invoke();
            yield return null;
        }
        #endregion

        public static IEnumerator CooldownSequence(CooldownData data, float cooldown, int maxCharges)
        {
            data.isRecharging = true;
            data.cooldownDelta = cooldown;
            while (data.currentCharges < maxCharges)
            {
                yield return null;
                data.cooldownDelta -= Time.deltaTime;

                if (data.cooldownDelta <= 0)
                {
                    data.currentCharges++;
                    data.cooldownDelta = cooldown;
                }
            }
            data.isRecharging = false;
        }
    }
}