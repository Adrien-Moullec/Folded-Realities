using System;
using System.Collections;
using UnityEngine;

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
        }


        protected virtual void OnDrawGizmos()
        {
            if (entityBody.feet == null || !drawGizmos) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(entityBody.feet.transform.position + entityBody.feet.center, entityBody.feet.radius);
        }
        #region Input Interface
        public abstract void InputMove(Vector3 direction, bool isDashing);
        public abstract void InputPrimaryAttack();
        public abstract void InputPrimaryAbility();
        #endregion

        #region Ability Interface
        public abstract void OnRotateEntity(Vector3 movement);
        public abstract void OnMoveEntity(Vector3 direction);
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
            Debug.Log("Cooldown End");
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
                    data.currentCharges = Mathf.Min(maxCharges, data.currentCharges + 1);
                    data.cooldownDelta = cooldown;
                }
            }
            data.isRecharging = false;
        }
    }
}