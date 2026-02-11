using UnityEngine;
using System;

public abstract class ActivatedAbilitySO : BaseAbility
{
    public abstract void Activate(PlayerAbilityController ab, AbilitySummary abs);
}

[Serializable]
public class ActivatedAbilitySummary : AbilitySummary
{
    [SerializeField] internal ActivatedAbilitySO abilitySO;
    internal void Activate(PlayerAbilityController ab, AbilitySummary abs) => abilitySO.Activate(ab, abs);
}