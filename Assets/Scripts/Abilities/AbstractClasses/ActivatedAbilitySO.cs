using UnityEngine;

public abstract class ActivatedAbilitySO : BaseAbility
{
    public abstract void Activate(AbilityController ab, AbilitySummary abs);
}