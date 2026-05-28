using UnityEngine;

public class HealthCollectible : MonoBehaviour {

    void OnTriggerEnter(Collider other) {

        if (!other.CompareTag("Player")) {
            return;
        }

        if (other.TryGetComponent(out IHealth ihealth)) {
            ihealth.SetMaxHealth();
            Destroy(gameObject);
        }
        /*
                IHealth ihealth =
                    other.GetComponentInChildren<IHealth>();

                if (ihealth != null) {

                    ihealth.Heal(
                        new AbilitySystem.EntityDamage(20, null)
                    );

                } else {
                    Debug.LogWarning("No IHealth found on player");
                }*/
    }
}