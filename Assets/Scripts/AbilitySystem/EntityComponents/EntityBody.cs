using System;
using System.Collections.Generic;

using UnityEngine;


namespace AbilitySystem {

    /// <summary>
    /// Important data containter about the entity to be passed through abilities for reference to important base data.
    /// </summary>
    [Serializable]
    public class EntityBody {
        [Header("Body")]
        [Tooltip("Gameobject that parents the body prefab.")]
        public GameObject bodyHolder;
        [Tooltip("Animator Manager reference.")]
        public CharacterAnimatorManager animatorManager;
        [Tooltip("Prefab base reference.")]
        public Transform prefab;
        [Tooltip("List of Renderers to manipulate shaders.")]
        public List<Renderer> entityShader;

        [Header("Ability References")]
        [Tooltip("Used ability set containing ability data.")]
        [HideInInspector] public AbilitySet abilitySet;
        [Tooltip("Interface reference for the ability controller movement.")]
        [HideInInspector] public IAbility iAbility;
        [Tooltip("Interface reference for the ability controller health.")]
        [HideInInspector] public IHealth iHealth;

        [Header("Get Info")]
        [Tooltip("Returns the grounded state of the entity.")]
        [HideInInspector] public bool isGrounded { get => iAbility.IsGrounded(); }
        [Tooltip("Whether the entity is already using an ability.")]
        [HideInInspector] public bool UsingAbility = false;
        [Tooltip("Whether an ability is currently overriding the value inputs.")]
        [HideInInspector] public bool MoveOverride = false;
    }
}