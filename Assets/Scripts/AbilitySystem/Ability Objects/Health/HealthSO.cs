using System;

using UnityEngine;


namespace AbilitySystem {
    /// <summary>
    /// Base health 'ability' to allow entities to have different abilities or events occur during health/damage scenarios.
    /// </summary>
    [CreateAssetMenu(fileName = "Health", menuName = MenuAssetNames.Health)]
    public class HealthSO : AbilitySO {

        [Header("Health")]
        [Tooltip("Defense. Damage dealt is calculated by attack damage/defense. Defense of 0.5 would double the damage. Defense of 2 would half the damage.")]
        [SerializeField] public float Defense = 1;

        /// <summary>
        /// Set max health.
        /// </summary>
        public void MaxHealth(EntityBody entityBody, ref int currentHealth, ref int maxHealth) => currentHealth = maxHealth;

        /// <summary>
        /// Heal an amount if healer is on the same team.
        /// </summary>
        public void HealAmount(EntityBody entityBody, ref int currentHealth, ref int maxHealth, EntityDamage entityDamage) {
            if (!EntityTeamFunctions.HasCommonTeam(entityBody.iAbility.GetEntityTeam, entityDamage.damagingTeam)) return;
            currentHealth = Mathf.Clamp(currentHealth + (int)entityDamage.amount, 0, maxHealth);
        }

        /// <summary>
        /// Damage an amount if damager is on a different team.
        /// </summary>
        public void DamageAmount(EntityBody entityBody, ref int currentHealth, ref int maxHealth, EntityDamage entityDamage) {
            if (EntityTeamFunctions.HasCommonTeam(entityBody.iAbility.GetEntityTeam, entityDamage.damagingTeam)) return;
            currentHealth = Mathf.Clamp(currentHealth - (int)(entityDamage.amount / Defense), 0, maxHealth);
        }

        /// <summary>
        /// Default healing ability.
        /// </summary>
        public static void DefaultHeal(EntityBody entityBody, ref int currentHealth, ref int maxHealth, EntityDamage entityDamage) {
            if (!EntityTeamFunctions.HasCommonTeam(entityBody.iAbility.GetEntityTeam, entityDamage.damagingTeam)) return;
            currentHealth = Mathf.Clamp(currentHealth + (int)entityDamage.amount, 0, maxHealth);
        }

        /// <summary>
        /// Default damage ability.
        /// </summary>
        public static void DefaultDamage(EntityBody entityBody, ref int currentHealth, ref int maxHealth, EntityDamage entityDamage) {
            if (EntityTeamFunctions.HasCommonTeam(entityBody.iAbility.GetEntityTeam, entityDamage.damagingTeam)) return;
            currentHealth = Mathf.Clamp(currentHealth - (int)entityDamage.amount, 0, maxHealth);
        }

        /// <summary>
        /// Default death ability.
        /// </summary>
        public void Die(EntityBody entityBody, ref int currentHealth) => currentHealth = 0;

    }
}