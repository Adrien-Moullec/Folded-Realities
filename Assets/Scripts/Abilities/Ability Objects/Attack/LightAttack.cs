using System;
using System.Collections;

using UnityEngine;


namespace AbilitySystem {
    [CreateAssetMenu(fileName = "Light Attack", menuName = MenuAssetNames.AttackAbility + "/Light attack")]
    public class LightAttack : CooldownAbilitySO {
        [SerializeField] int damage = 10;
        public override AbilityData AbilityDataSetup() => new CooldownData(charges, cooldown);

        protected override IEnumerator Ability(EntityBody entityBody, CooldownData data) {

            Collider[] colliders = Physics.OverlapSphere(entityBody.attackCubeArea.transform.position, entityBody.attackCubeArea.size.x);
            foreach (var n in colliders)
                if (n.transform.TryGetComponent(out IHealth iHealth))
                    if (iHealth != entityBody.iHealth)
                        iHealth.Damage(damage);

            yield return null;
        }
    }
}