using System;

using UnityEngine;


namespace AbilitySystem {
    [Serializable]
    public class EntityBody {
        [Header("Body")]
        [Tooltip("Gameobject that parents the body prefab.")]
        public GameObject bodyHolder;
        [Tooltip("Gameobject of the prefab body.")]
        public GameObject modelPrefab;
        [Tooltip("Gameobject that has the Collider for the feet collisions.")]
        public SphereCollider feetSphereArea;
        [Tooltip("Attack cube area.")]
        public BoxCollider attackCubeArea;

        [Space]
        [Header("Animation References")]
        [Tooltip("Reference to the component that animates the body prefab.")]
        public Transform upperBody;
        public Transform lowerBody;
        public Animation animationComponent;

        [Space]
        [Header("References")]
        [Tooltip("Interface reference for the Ability Controller.")]
        [HideInInspector] public IAbility iAbility;
    }
}