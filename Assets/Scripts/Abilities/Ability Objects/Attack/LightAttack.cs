using System;
using System.Collections;

using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Light Attack", menuName = "Origami/Light Attack/Generic Light Attack")]
    public class LightAttack : CooldownAbilitySO {
        public override AbilityData AbilityDataSetup() => new CooldownData(charges, cooldown);

        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {
            Debug.Log("USE ABILITY");
            yield return null;
        }
    }
}