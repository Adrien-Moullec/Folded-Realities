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
        public virtual IEnumerator RunAnimationWithEvent(AbilityAnimation abilityAnimation, Animation animationComponent, IEnumerator ability)
        {
            animationComponent.Play(abilityAnimation.animation.name);
            AnimationState state = animationComponent[abilityAnimation.animation.name];
            Debug.Log("State activated");
            float normalizedTime;

            while (state.enabled)
            {
                normalizedTime = state.normalizedTime % 1f;
                state.weight = abilityAnimation.weightOverTime.Evaluate(normalizedTime);
                if (normalizedTime >= abilityAnimation.abilityEventDelta)
                {
                    Debug.Log("ABILITY");
                    yield return StartCoroutine(ability);
                    yield break;
                }

                yield return null;
            }
            while (state.enabled) yield return null;
            Debug.Log("End ability");
        }
        public virtual IEnumerator RunAnimationWithEvent(AbilityAnimation abilityAnimation, Animation animationComponent, Action ability) => RunAnimationWithEvent(abilityAnimation, animationComponent, ActionToIenumerator(ability));
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