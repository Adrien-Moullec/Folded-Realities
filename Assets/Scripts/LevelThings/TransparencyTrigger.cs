using UnityEngine;

public class TransparencyTrigger : MonoBehaviour {
    [SerializeField] private ObjectTransparency targetObject;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(playerTag)) {
            targetObject.SetTransparent(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(playerTag)) {
            targetObject.SetTransparent(false);
        }
    }
}