using UnityEngine;

using System.Collections;

using AbilitySystem;

public class DamageZone : MonoBehaviour {

    public int damageAmount = 10;

    public float damageDelay = 1f;

    bool damagingPlayer = false;

    IHealth iPlayerHealth;

    Coroutine damageRoutine;

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

        iPlayerHealth =
            other.GetComponent<
                IHealth
            >();

        if (
            iPlayerHealth == null
        ) {
            return;
        }

        if (
            damagingPlayer
        ) {
            return;
        }

        damagingPlayer = true;

        damageRoutine =
            StartCoroutine(
                DamageRoutine()
            );
    }

    void OnTriggerExit(
        Collider other
    ) {

        if (
            !other.CompareTag(
                "Player"
            )
        ) {
            return;
        }

        damagingPlayer = false;

        if (
            damageRoutine != null
        ) {
            StopCoroutine(
                damageRoutine
            );
        }
    }

    IEnumerator DamageRoutine() {

        while (
            damagingPlayer
        ) {

            if (
                iPlayerHealth != null
            ) {

                iPlayerHealth.Damage(
                    new EntityDamage(damageAmount, null)
                );
            }

            yield return new WaitForSeconds(
                damageDelay
            );
        }
    }
}