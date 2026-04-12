using System;

using UnityEngine;


namespace AbilitySystem {
    [Serializable]
    public class EntityBody {
        [Header("Body")]
        [Tooltip("Gameobject that parents the body prefab.")]
        public GameObject bodyHolder;
        [Tooltip("Gameobject that has the Collider for the feet collisions.")]
        public SphereCollider feetSphereArea;
        [Tooltip("Attack cube area.")]
        public BoxCollider attackCubeArea;
        [Tooltip("AnimatorManager.")]
        public AnimatorManager animatorManager;

        [Space]
        [Header("Ability References")]
        [HideInInspector] public AbilitySet abilitySet;
        [Tooltip("Interface reference for the Ability Controller.")]
        [HideInInspector] public IAbility iAbility;
        [HideInInspector] public IHealth iHealth;
        [HideInInspector] public bool UsingAbility = false;
        [HideInInspector] public bool MoveOverride = false;
    }
}