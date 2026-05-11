using System;
using System.Collections.Generic;

using UnityEngine;


namespace AbilitySystem {
    [Serializable]
    public class EntityBody {
        [Header("Body")]
        [Tooltip("Gameobject that parents the body prefab.")]
        public GameObject bodyHolder;
        [Tooltip("Animator Manager.")]
        public AnimatorManager animatorManager;
        public List<Renderer> entityShader;

        [Header("Ability References")]
        [HideInInspector] public AbilitySet abilitySet;
        [Tooltip("Interface reference for the Ability Controller.")]
        [HideInInspector] public IAbility iAbility;
        [HideInInspector] public IHealth iHealth;

        [Header("Get Info")]
        [HideInInspector] public bool isGrounded { get => iAbility.IsGrounded(); }
        [HideInInspector] public bool UsingAbility = false;
        [HideInInspector] public bool MoveOverride = false;
    }
}