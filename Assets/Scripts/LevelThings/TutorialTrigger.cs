using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class TutorialTrigger : MonoBehaviour {
    public GameObject tutorialUI;
    public float delayBeforeUI = 2f;

    public CinemachineCamera gameplayCam;
    public CinemachineCamera focusCam;

    private bool triggered = false;

    void OnTriggerEnter(Collider other) {
        Debug.Log("Triggered by: " + other.name);

        if (other.CompareTag("Player") && !triggered) {
            triggered = true;
            StartCoroutine(TutorialSequence(other.gameObject));
        }
    }

    IEnumerator TutorialSequence(GameObject player) {
        CharacterController controller = player.GetComponent<CharacterController>();
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (controller != null)
            controller.enabled = false;

        if (rb != null) {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        gameplayCam.Priority = 0;
        focusCam.Priority = 20;

        yield return new WaitForSeconds(delayBeforeUI);

        if (tutorialUI != null) {
            Debug.Log("Showing UI");
            tutorialUI.SetActive(true);
        }

        yield return new WaitUntil(() => Input.anyKeyDown);

        if (tutorialUI != null)
            tutorialUI.SetActive(false);

        focusCam.Priority = 0;
        gameplayCam.Priority = 20;

        if (controller != null)
            controller.enabled = true;

        if (rb != null)
            rb.isKinematic = false;
    }
}