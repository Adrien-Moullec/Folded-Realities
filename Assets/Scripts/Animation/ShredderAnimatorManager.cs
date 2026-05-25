using AbilitySystem;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShredderAnimatorManager : BaseAnimatorManager {
    protected override int GetLayerInfo(string input) => 0;
    protected override bool CanStartAnimation((int hashCode, int layer) info) => true;
    protected override void ActOnAnimatorStateInfoReceiveEvent(AnimationEvent animationEvent, AnimatorStateInfo stateInfo, int i) {
        layers[i].state.eventFunction?.Invoke(new AbilityAnimationEventData(animationEvent, stateInfo.normalizedTime % 1));
    }
}
public enum ShredderAnim {
    WakeUp,
    Idle,
    Hit,
    SpitCharge,
    SpinAttack,
    Spit,
    Sleep,
    Death
}