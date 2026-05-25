using UnityEngine;

using System.Collections;

public class KillZone : MonoBehaviour {

    bool triggered = false;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || triggered)
            return;

        triggered = true;
        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;
        StartCoroutine(KillRoutine(other.gameObject, controller));
    }

    IEnumerator KillRoutine(GameObject player, CharacterController controller) {
        yield return GameplaySystem.instance.Respawn(true, TransitionType.Iris);
        triggered = false;
    }
}