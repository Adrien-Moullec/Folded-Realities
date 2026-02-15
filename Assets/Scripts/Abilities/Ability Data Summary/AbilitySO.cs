using System;
using System.Collections;
using UnityEngine;


namespace AbilitySystem
{
    public abstract class AbilitySO : ScriptableObject
    {
        public abstract AbilityData Setup();
    }
}