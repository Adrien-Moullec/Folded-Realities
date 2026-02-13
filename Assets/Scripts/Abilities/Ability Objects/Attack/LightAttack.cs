using System.Collections;
using UnityEngine;


namespace AbilitySystem
{
    public class LightAttack : CooldownAbilitySO
    {
        public override AbilityData Setup() => null;

        public override void Activate(EntityBody entityBody, AbilityData data)
        {
            base.Activate(entityBody, data);
        }
    }
}