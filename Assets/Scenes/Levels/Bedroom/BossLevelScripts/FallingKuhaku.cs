using UnityEngine;
using System.Collections;

public class FallingKuhaku : MonoBehaviour {
    public CharacterController controller;

    public float moveSpeed = 8f;

    public float fallSpeed = 12f;

    public float slowedMoveMultiplier = 0.3f;

    Vector3 velocity;

    bool slowed = false;

    bool inTransition = false;

    void Update() {
        if (
            inTransition
        ) {
            return;
        }

        if (
            controller == null
            || !controller.enabled
        ) {
            return;
        }

        float currentMoveSpeed =
            slowed
            ? moveSpeed
                * slowedMoveMultiplier
            : moveSpeed;

        float h =
            Input.GetAxis(
                "Horizontal"
            );

        Vector3 move =
            new Vector3(
                h,
                0f,
                0f
            );

        controller.Move(
            move
            * currentMoveSpeed
            * Time.deltaTime
        );

        velocity.y = -fallSpeed;

        controller.Move(
            velocity
            * Time.deltaTime
        );
    }

    public void HitSlowdown() {
        if (
            !gameObject.activeInHierarchy
        ) {
            return;
        }

        StopAllCoroutines();

        StartCoroutine(
            SlowRoutine()
        );
    }

    IEnumerator SlowRoutine() {
        slowed = true;

        yield return new WaitForSeconds(
            0.4f
        );

        slowed = false;
    }

    public void SetTransitionState(
        bool state
    ) {
        inTransition = state;
    }
}