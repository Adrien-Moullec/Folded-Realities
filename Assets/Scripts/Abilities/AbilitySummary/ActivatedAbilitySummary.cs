using UnityEngine;
using System;

[Serializable]
public class ActivatedAbilitySummary : AbilitySummary
{
    [SerializeField] internal ActivatedAbilitySO abilitySO;
    internal void Activate(AbilityController ab, AbilitySummary abs) => abilitySO.Activate(ab, abs);
}