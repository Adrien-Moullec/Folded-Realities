using System;

using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Health", menuName = MenuAssetNames.Health)]
    public class HealthSO : AbilitySO {

        [Header("Health")]
        [Tooltip("Defense. Damage dealt is calculated by attack damage/defense. Defense of 0.5 would double the damage. Defense of 2 would half the damage.")]
        [SerializeField] public float Defense = 1;

        public void MaxHealth(EntityBody entityBody, ref int currentHealth, ref int maxHealth) {
            currentHealth = maxHealth;
        }
        public void HealAmount(EntityBody entityBody, ref int currentHealth, ref int maxHealth, EntityDamage entityDamage) {
            if (!EntityTeamFunctions.HasCommonTeam(entityBody.iAbility.GetEntityTeam, entityDamage.damagingTeam)) return;
            currentHealth = Mathf.Clamp(currentHealth + (int)entityDamage.amount, 0, maxHealth);
        }
        public void DamageAmount(EntityBody entityBody, ref int currentHealth, ref int maxHealth, EntityDamage entityDamage) {
            if (EntityTeamFunctions.HasCommonTeam(entityBody.iAbility.GetEntityTeam, entityDamage.damagingTeam)) return;
            currentHealth = Mathf.Clamp(currentHealth - (int)(entityDamage.amount / Defense), 0, maxHealth);
        }
        public static void DefaultHeal(EntityBody entityBody, ref int currentHealth, ref int maxHealth, EntityDamage entityDamage) {
            if (!EntityTeamFunctions.HasCommonTeam(entityBody.iAbility.GetEntityTeam, entityDamage.damagingTeam)) return;
            currentHealth = Mathf.Clamp(currentHealth + (int)entityDamage.amount, 0, maxHealth);
        }
        public static void DefaultDamage(EntityBody entityBody, ref int currentHealth, ref int maxHealth, EntityDamage entityDamage) {
            if (EntityTeamFunctions.HasCommonTeam(entityBody.iAbility.GetEntityTeam, entityDamage.damagingTeam)) return;
            currentHealth = Mathf.Clamp(currentHealth - (int)entityDamage.amount, 0, maxHealth);
        }
        public void Die(EntityBody entityBody, ref int currentHealth) {
            currentHealth = 0;
        }
    }
}