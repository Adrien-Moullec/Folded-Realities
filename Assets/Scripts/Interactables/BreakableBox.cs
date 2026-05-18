using UnityEngine;
using System.Collections;
using AbilitySystem;

public class BreakableBox : MonoBehaviour,
    IHealth {
    public GameObject[] starPrefabs;

    public int hitsToBreak = 3;

    public int starsToSpawn = 10;

    public float spawnDelay = 0.07f;

    public float upwardForce = 7f;

    public float outwardForce = 4f;

    int currentHits = 0;

    bool isBroken = false;

    public void Damage(
        EntityDamage damage
    ) {
        if (
            isBroken
        ) {
            return;
        }

        currentHits++;

        if (
            currentHits >= hitsToBreak
        ) {
            StartCoroutine(
                BreakBox()
            );
        }
    }

    public void Heal(
        EntityDamage heal
    ) {
    }

    public void Die() {
        if (
            isBroken
        ) {
            return;
        }

        StartCoroutine(
            BreakBox()
        );
    }

    IEnumerator BreakBox() {
        isBroken = true;

        Vector3 spawnPoint =
            transform.position
            + Vector3.up;

        MeshRenderer renderer =
            GetComponent<MeshRenderer>();

        if (
            renderer != null
        ) {
            renderer.enabled = false;
        }

        Collider col =
            GetComponent<Collider>();

        if (
            col != null
        ) {
            col.enabled = false;
        }

        for (
            int i = 0;
            i < starsToSpawn;
            i++
        ) {
            GameObject starPrefab =
                starPrefabs[
                    Random.Range(
                        0,
                        starPrefabs.Length
                    )
                ];

            GameObject star =
                Instantiate(
                    starPrefab,
                    spawnPoint,
                    Quaternion.identity
                );

            Rigidbody rb =
                star.GetComponent<Rigidbody>();

            if (
                rb != null
            ) {
                float angle =
                    (360f / starsToSpawn)
                    * i
                    * Mathf.Deg2Rad;

                Vector3 outward =
                    new Vector3(
                        Mathf.Cos(angle),
                        0,
                        Mathf.Sin(angle)
                    );

                Vector3 launch =
                    Vector3.up
                    * upwardForce
                    + outward
                    * outwardForce;

                rb.AddForce(
                    launch,
                    ForceMode.Impulse
                );

                rb.AddTorque(
                    Random.insideUnitSphere
                    * 10f,
                    ForceMode.Impulse
                );
            }

            yield return new WaitForSeconds(
                spawnDelay
            );
        }

        Destroy(
            gameObject
        );
    }
}