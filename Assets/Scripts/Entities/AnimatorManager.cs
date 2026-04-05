using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorManager : MonoBehaviour {
    private Animator animator;

    #region Parameters
    public const string deltaSpeed = "deltaSpeed";
    public const string deltaFall = "deltaFall";
    public const string isFalling = "isFalling";
    #endregion

    #region States
    public void SetMovement(float dSpeed, float dFall, bool isFalling) {
        animator.SetFloat("deltaSpeed", Mathf.Clamp01(dSpeed));
        animator.SetFloat("deltaFall", Mathf.Clamp01(dFall));
        animator.SetBool("isFalling", isFalling);
    }
    #endregion

    void Awake() {
        animator = GetComponent<Animator>();
    }

    #region OUTTAKE
    /* 
    public void PlayAbility(AbilityAnimations animations) {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(PlaySequence(animations));
    }

    private IEnumerator PlaySequence(AbilityAnimations animations) {
        if (!string.IsNullOrEmpty(animations.startTrigger)) {
            animator.SetTrigger(animations.startTrigger);
            yield return WaitForAnimation();
        }

        if (animations.attackTriggers != null) {
            foreach (var trigger in animations.attackTriggers) {
                animator.SetTrigger(trigger);
                yield return WaitForAnimation();
            }
        }

        if (!string.IsNullOrEmpty(animations.endTrigger)) {
            animator.SetTrigger(animations.endTrigger);
        }
    }

    private IEnumerator WaitForAnimation() {
        yield return null;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        while (state.normalizedTime < 1f) {
            state = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
    }*/
    #endregion
}