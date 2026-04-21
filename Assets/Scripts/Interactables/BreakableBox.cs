using UnityEngine;
using System.Collections;

public class BreakableBox : MonoBehaviour {
    public GameObject[] starPrefabs;

    public int hitsToBreak = 3;
    public int starsToSpawn = 10;

    public float spawnDelay = 0.07f;

    public float upwardForce = 7f;
    public float outwardForce = 4f;

    int currentHits = 0;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        currentHits++;

        if (currentHits >= hitsToBreak) {
            StartCoroutine(BreakBox());
        }
    }

    IEnumerator BreakBox() {
        Vector3 spawnPoint = transform.parent.position + Vector3.up;

        // hide box
        transform.parent.GetComponent<MeshRenderer>().enabled = false;
        transform.parent.GetComponent<Collider>().enabled = false;

        for (int i = 0; i < starsToSpawn; i++) {
            // choose random star colour
            GameObject starPrefab = starPrefabs[Random.Range(0, starPrefabs.Length)];

            GameObject star = Instantiate(starPrefab, spawnPoint, Quaternion.identity);

            Rigidbody rb = star.GetComponent<Rigidbody>();

            if (rb != null) {
                // evenly spaced circular direction
                float angle = (360f / starsToSpawn) * i * Mathf.Deg2Rad;

                Vector3 outward = new Vector3(
                    Mathf.Cos(angle),
                    0,
                    Mathf.Sin(angle)
                );

                Vector3 launch = Vector3.up * upwardForce + outward * outwardForce;

                rb.AddForce(launch, ForceMode.Impulse);

                rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(spawnDelay);
        }

        Destroy(transform.parent.gameObject);
    }
}