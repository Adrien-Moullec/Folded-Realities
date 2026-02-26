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

        //[HideInInspector] public Animation animation; //Having this here wouldn't work for multiple bodies
        [HideInInspector] public IAbility iAbility;
    }
}