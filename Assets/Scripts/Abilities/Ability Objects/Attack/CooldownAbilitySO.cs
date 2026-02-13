using UnityEngine;
using System;
using System.Collections;


namespace AbilitySystem
{
    public abstract class CooldownAbilitySO : AbilitySO
    {
        [Header("Ability Settings")]
        [SerializeField] internal float cooldown;
        [SerializeField] internal int charges;
        public virtual void Activate(EntityBody entityBody, AbilityData data)
        {
            if (data.currentCharges <= 0) return;
            entityBody.iAbilityCooldown.OnAbilityUsed(data);
        }
        public static IEnumerator Cooldown(AbilityData data, int maxCharges, float cooldown)
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

    [Serializable]
    public class ActivatedAbilitySummary : AbilitySummary
    {
        [SerializeField] internal CooldownAbilitySO abilitySO;
        internal void Activate(EntityBody entityBody, AbilityData data) => abilitySO.Activate(entityBody, data);
    }
}