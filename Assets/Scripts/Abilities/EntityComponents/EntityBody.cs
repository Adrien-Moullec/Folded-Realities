using System;
using UnityEngine;


namespace AbilitySystem
{
    [Serializable]
    public class EntityBody
    {
        public GameObject body;
        public SphereCollider feet;
        [HideInInspector] public IMovement iMovement;
        [HideInInspector] public IAbilityCooldown iAbilityCooldown;
    }
}