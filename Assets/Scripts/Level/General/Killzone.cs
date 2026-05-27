using UnityEngine;

using System.Collections;

public class KillZone : MonoBehaviour {
    // Prevents multiple trigger activations
    bool triggered = false;
    // Only activates for player once
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || triggered)
            return;
        // Disables movement controller before respawn
        triggered = true;
        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;
        // Starts respawn sequence
        StartCoroutine(KillRoutine(other.gameObject, controller));
    }

    IEnumerator KillRoutine(GameObject player, CharacterController controller) {
        yield return GameplaySystem.instance.Respawn(true, TransitionType.Iris);
        triggered = false;
    }
}