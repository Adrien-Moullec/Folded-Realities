using AbilitySystem;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShredderAnimatorManager : BaseAnimatorManager {
    protected override int GetLayerInfo(string input) => 0;
    protected override bool CanStartAnimation((int hashCode, int layer) info) => true;
}
public enum ShredderAnim {
    WakeUp,
    Idle,
    Hit,
    SpinAttack,
    SpitCharge,
    SpitHold,
    Spit,
    Sleep,
    Death
}