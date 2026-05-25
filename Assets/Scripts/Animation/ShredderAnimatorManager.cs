using AbilitySystem;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShredderAnimatorManager : BaseAnimatorManager {
    protected override int GetLayerInfo(string input) => 1;
    protected override bool CanStartAnimation((int hashCode, int layer) info) => true;
}
public enum ShredderAnims {
    WakeUp,
    Idle,
    Hit,
    SpitCharge,
    SpitHold,
    Spit,
    Sleep,
    Death
}