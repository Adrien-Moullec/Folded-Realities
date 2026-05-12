using UnityEngine;

public class TriggerDebugger : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        Debug.Log("Entered Trigger: " + other.name);
    }
}