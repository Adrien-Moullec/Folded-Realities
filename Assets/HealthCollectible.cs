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

        if (other.TryGetComponent(out IHealth ihealth)) {
            ihealth.Heal(new AbilitySystem.EntityDamage(20, null));
        }

        /*if (
            CollectiblesManager.Instance
            != null
        ) {

            CollectiblesManager.Instance.CollectHealth(
                gameObject,
                other.gameObject
            );
        }*/
    }
}