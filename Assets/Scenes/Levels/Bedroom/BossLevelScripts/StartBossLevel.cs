using UnityEngine;

public class StartBossLevel : MonoBehaviour {
    public GameObject[] debrisPrefabs;

    public BoxCollider spawnArea;

    public int debrisCount = 10;

    public float moveSpeed = 8f;

    GameObject[] spawnedDebris;

    void Start() {
        spawnedDebris =
            new GameObject[debrisCount];

        Bounds bounds =
            spawnArea.bounds;

        for (int i = 0; i < debrisCount; i++) {
            Vector3 pos =
                new Vector3(
                    Random.Range(
                        bounds.min.x,
                        bounds.max.x
                    ),
                    Random.Range(
                        bounds.min.y,
                        bounds.max.y
                    ),
                    Random.Range(
                        bounds.min.z,
                        bounds.max.z
                    )
                );

            GameObject prefab =
                debrisPrefabs[
                    Random.Range(
                        0,
                        debrisPrefabs.Length
                    )
                ];

            GameObject debris =
                Instantiate(
                    prefab,
                    pos,
                    Random.rotation
                );

            float scale =
                Random.Range(1.5f, 4f);

            debris.transform.localScale =
                Vector3.one * scale;

            spawnedDebris[i] = debris;
        }
    }

    void Update() {
        if (spawnArea == null) {
            return;
        }

        Bounds bounds =
            spawnArea.bounds;

        foreach (GameObject debris in spawnedDebris) {
            if (debris == null) {
                continue;
            }

            debris.transform.position +=
                Vector3.up
                * moveSpeed
                * Time.deltaTime;

            if (
                debris.transform.position.y
                > bounds.max.y
            ) {
                Vector3 pos =
                    debris.transform.position;

                pos.y = bounds.min.y;

                pos.x =
                    Random.Range(
                        bounds.min.x,
                        bounds.max.x
                    );

                pos.z =
                    Random.Range(
                        bounds.min.z,
                        bounds.max.z
                    );

                debris.transform.position = pos;
            }
        }
    }
}