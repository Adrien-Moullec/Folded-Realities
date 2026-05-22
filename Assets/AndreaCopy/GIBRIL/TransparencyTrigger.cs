using UnityEngine;

public class TransparencyTrigger : MonoBehaviour {

    [Header("Transparency")]
    [SerializeField] private ObjectTransparency targetObject;

    [Header("Enable / Disable")]
    [SerializeField] private GameObject targetGameObject;

    [SerializeField] private bool disableWhenPlayerInside = true;

    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other) {

        if (!other.CompareTag(playerTag))
            return;

        // transparency
        if (targetObject != null) {
            targetObject.SetTransparent(true);
        }

        // enable / disable object
        if (targetGameObject != null) {

            if (disableWhenPlayerInside) {
                targetGameObject.SetActive(false);
            } else {
                targetGameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) {

        if (!other.CompareTag(playerTag))
            return;

        // transparency
        if (targetObject != null) {
            targetObject.SetTransparent(false);
        }

        // enable / disable object
        if (targetGameObject != null) {

            if (disableWhenPlayerInside) {
                targetGameObject.SetActive(true);
            } else {
                targetGameObject.SetActive(false);
            }
        }
    }
}