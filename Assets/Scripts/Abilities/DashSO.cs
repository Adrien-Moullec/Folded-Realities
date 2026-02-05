using UnityEngine;

public abstract class DashSO : ScriptableObject
{
    public abstract void Dash(AbilityController abilitySummary, Vector3 dir);
}
