using UnityEngine;

public class CollectiblePickup : MonoBehaviour {
    public bool isSpecial = false;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        CollectiblesManager manager = other.GetComponent<CollectiblesManager>();

        if (manager == null) {
            return;
        }

        if (isSpecial) {
            manager.CollectSpecial(gameObject);
        } else {
            manager.CollectNormal(gameObject);
        }
    }
}