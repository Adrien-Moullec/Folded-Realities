using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class TutorialTrigger : MonoBehaviour {

    public GameObject tutorialUI;

    // Delay before tutorial UI appears
    public float delayBeforeUI = 2f;

    public CinemachineCamera gameplayCam;

    public CinemachineCamera focusCam;

    bool triggered = false;

    void OnTriggerEnter(Collider other) {

        // Starts tutorial sequence once
        if (other.CompareTag("Player") && !triggered) {

            triggered = true;

            StartCoroutine(
                TutorialSequence(other.gameObject)
            );
        }
    }

    IEnumerator TutorialSequence(GameObject player) {

        CharacterController controller =
            player.GetComponent<CharacterController>();

        Rigidbody rb =
            player.GetComponent<Rigidbody>();

        // Freezes player movement
        if (controller != null)
            controller.enabled = false;

        if (rb != null) {

            rb.linearVelocity = Vector3.zero;

            rb.isKinematic = true;
        }

        // Switches to focus camera
        gameplayCam.Priority = 0;

        focusCam.Priority = 20;

        yield return new WaitForSeconds(delayBeforeUI);

        // Shows tutorial UI
        if (tutorialUI != null)
            tutorialUI.SetActive(true);

        yield return new WaitUntil(() => Input.anyKeyDown);

        // Hides UI and restores gameplay camera
        if (tutorialUI != null)
            tutorialUI.SetActive(false);

        focusCam.Priority = 0;

        gameplayCam.Priority = 20;

        // Re-enables player movement
        if (controller != null)
            controller.enabled = true;

        if (rb != null)
            rb.isKinematic = false;
    }
}