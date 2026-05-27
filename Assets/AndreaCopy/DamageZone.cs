using UnityEngine;
using System.Collections;
using AbilitySystem;

public class DamageZone : MonoBehaviour {

    public int damageAmount = 10;

    // Delay between damage 
    public float damageDelay = 1f;

    bool damagingPlayer = false;

    IHealth iPlayerHealth;

    Coroutine damageRoutine;

    void OnTriggerEnter(Collider other) {

        // Only damages player
        if (!other.CompareTag("Player"))
            return;

        iPlayerHealth = other.GetComponent<IHealth>();

        if (iPlayerHealth == null || damagingPlayer)
            return;

        damagingPlayer = true;

        damageRoutine = StartCoroutine(DamageRoutine());
    }

    void OnTriggerExit(Collider other) {

        if (!other.CompareTag("Player"))
            return;

        damagingPlayer = false;

        // Stops damage coroutine on exit
        if (damageRoutine != null)
            StopCoroutine(damageRoutine);
    }

    IEnumerator DamageRoutine() {

        // Applies repeated damage while inside zone
        while (damagingPlayer) {

            if (iPlayerHealth != null)
                iPlayerHealth.Damage(
                    new EntityDamage(damageAmount, null)
                );

            yield return new WaitForSeconds(damageDelay);
        }
    }
}