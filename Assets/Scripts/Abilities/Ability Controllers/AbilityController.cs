using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace AbilitySystem
{
    public abstract class AbilityController : MonoBehaviour, IAbility
    {
        [Header("Body Components")]
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] public EntityBody entityBody;

        protected virtual void Awake()
        {
            entityBody.iAbility = this;
            SetupAnimations();
        }

        internal abstract void SetupAnimations();

        protected virtual void OnDrawGizmos()
        {
            if (entityBody.feet == null || !drawGizmos) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(entityBody.feet.transform.position + entityBody.feet.center, entityBody.feet.radius);
        }
        #region Input Interface
        public abstract void InputMove(Vector3 direction, bool isRunning);
        public abstract void InputPrimaryAttack();
        public abstract void InputPrimaryAbility();
        #endregion

        #region Ability Interface
        public abstract void OnRotateEntity(Vector3 movement);
        public abstract void OnMoveEntity(Vector3 direction, float turnSpeed = 1);
        public virtual void OnActivateCooldownAbility(IEnumerator ability, CooldownData data, float cooldown, int maxCharges)
        {
            StartCoroutine(RunAbility(ability, data));
            if (!data.isRecharging) StartCoroutine(CooldownSequence(data, cooldown, maxCharges));
        }
        public virtual IEnumerator RunAbility(IEnumerator ability, CooldownData data)
        {
            data.isUsing = true;
            Debug.Log("Start using ability");
            yield return StartCoroutine(ability);
            data.isUsing = false;
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
                    Debug.Log("COOLDOWN FINISHED ");
                    data.cooldownDelta = cooldown;
                }
            }
            data.isRecharging = false;
        }
    }
}