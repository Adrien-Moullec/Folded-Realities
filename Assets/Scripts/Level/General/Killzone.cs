using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour {

    bool triggered = false;

    private void OnTriggerEnter(
        Collider other
    ) {

        if (
            !other.CompareTag(
                "Player"
            )
        ) {
            return;
        }

        if (triggered) {
            return;
        }

        triggered = true;

        CharacterController controller =
            other.GetComponent<
                CharacterController
            >();

        if (controller != null) {

            controller.enabled = false;
        }

        StartCoroutine(
            KillRoutine(
                other.gameObject,
                controller
            )
        );
    }

    IEnumerator KillRoutine(
        GameObject player,
        CharacterController controller
    ) {

        if (
            SceneTransition.Instance
            == null
        ) {
            yield break;
        }

        yield return StartCoroutine(
            SceneTransition.Instance
                .RespawnTransition(
                    player
                )
        );

        yield return new WaitForSeconds(
            0.05f
        );

        if (controller != null) {

            controller.enabled = true;
        }

        triggered = false;
    }
}