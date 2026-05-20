using UnityEngine;

public class HealthCollectible : MonoBehaviour {

    void OnTriggerEnter(
        Collider other
    ) {

        if (
            !other.CompareTag(
                "Player"
            )
        ) {
            return;
        }

        if (
            CollectiblesManager.Instance
            != null
        ) {

            CollectiblesManager.Instance.CollectHealth(
                gameObject,
                other.gameObject
            );
        }
    }
}