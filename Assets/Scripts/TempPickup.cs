using AbilitySystem;

using UnityEngine;

public class TempPickup : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out PlayerAbilityController pac)) {
            if (pac.UnlockSet("Crane"))
                Destroy(gameObject);
        }
    }
}
