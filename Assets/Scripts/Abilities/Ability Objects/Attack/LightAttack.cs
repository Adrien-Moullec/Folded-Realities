using System;
using System.Collections;
using UnityEngine;


namespace AbilitySystem
{
    [CreateAssetMenu(fileName = "Light Attack", menuName = "Origami/Light Attack/Generic Light Attack")]
    public class LightAttack : CooldownAbilitySO
    {
        [SerializeField] float attackDuration = 1;
        public override AbilityData AbilityDataSetup() => new CooldownData(charges, cooldown);

        public override IEnumerator Ability(EntityBody entityBody, CooldownData data)
        {
            entityBody.model.GetComponent<Renderer>().material.color = Color.black;
            yield return new WaitForSeconds(attackDuration);
            entityBody.model.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}