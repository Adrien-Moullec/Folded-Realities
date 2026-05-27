using UnityEngine;

public class HealthCollectible : MonoBehaviour {

    void OnTriggerEnter(Collider other) {

        if (!other.CompareTag("Player")) {
            return;
        }

        IHealth ihealth =
            other.GetComponentInChildren<IHealth>();

        if (ihealth != null) {

            ihealth.Heal(
                new AbilitySystem.EntityDamage(20, null)
            );

            Destroy(gameObject);
        } else {
            Debug.LogWarning("No IHealth found on player");
        }
    }
}