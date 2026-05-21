using System.Collections;

using UnityEngine;

public class PlatformSpawner : MonoBehaviour {

    public GameObject platformPrefab;

    public float spawnRate = 1f;

    public int maxPlatforms = 20;

    int currentPlatforms = 0;

    BoxCollider spawnArea;

    void Start() {

        spawnArea =
            GetComponent<BoxCollider>();

        StartCoroutine(
            SpawnRoutine()
        );
    }

    IEnumerator SpawnRoutine() {

        while (true) {

            if (
                currentPlatforms <
                maxPlatforms
            ) {

                SpawnPlatform();
            }

            yield return new WaitForSeconds(
                spawnRate
            );
        }
    }

    void SpawnPlatform() {

        Vector3 center =
            spawnArea.bounds.center;

        Vector3 size =
            spawnArea.bounds.size;

        Vector3 randomPos =
            new Vector3(

                Random.Range(
                    center.x - size.x / 2,
                    center.x + size.x / 2
                ),

                Random.Range(
                    center.y - size.y / 2,
                    center.y + size.y / 2
                ),

                center.z +
                Random.Range(
                    -1f,
                    1f
                )
            );

        GameObject platform =
            Instantiate(
                platformPrefab,
                randomPos,
                platformPrefab.transform.rotation
            );

        currentPlatforms++;

        MovingPlatformDown mp =
            platform.GetComponent<
                MovingPlatformDown
            >();

        if (mp != null) {

            mp.spawner = this;

            BossFightManager manager =
                BossFightManager.Instance;

            if (manager != null) {

                mp.bossTarget =
                    manager.boss.transform;
            }
        }
    }

    public void PlatformDestroyed() {

        currentPlatforms--;
    }
}