using System;
using UnityEngine;


namespace AbilitySystem
{
    [Serializable]
    public class EntityBody
    {
        public GameObject bodyHolder;
        public GameObject model;
        public SphereCollider feet;
        [HideInInspector] public IAbility iAbility;
    }
}