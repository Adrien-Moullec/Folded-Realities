using UnityEngine;
using System.Collections.Generic;

public class DebrisSpawner : MonoBehaviour {
    [Header("Prefabs")]
    public GameObject[] debrisPrefabs;

    [Header("References")]
    public Transform player;
    public Transform npc;

    [Header("Spawn Settings")]
    public int spawnCount = 50;
    public Vector3 areaSize = new Vector3(20, 50, 20);

    [Header("Safety")]
    public float npcSafeRadius = 2f;
    public float playerSafeRadius = 2.5f;

    [Header("Spacing")]
    public float minDistanceBetweenDebris = 3f;

    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start() {
        
        for (int i = 0; i < spawnCount; i++) {
            SpawnDebris(false);
        }

       
        for (int i = 0; i < spawnCount; i++) {
            SpawnDebris(true);
        }
    }

    void SpawnDebris(bool bottomLayer) {
        Vector3 randomPos = transform.position;

        for (int i = 0; i < 15; i++) {
            float yMin = bottomLayer ? -areaSize.y / 2f : -areaSize.y / 4f;
            float yMax = bottomLayer ? -areaSize.y / 4f : areaSize.y / 2f;

            Vector3 testPos = transform.position + new Vector3(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                Random.Range(yMin, yMax),
                Random.Range(-areaSize.z / 2f, areaSize.z / 2f)
            );

            bool tooCloseToNPC = npc != null &&
                Vector3.Distance(testPos, npc.position) < npcSafeRadius;

            
            bool tooCloseToPlayer = player != null &&
                Mathf.Abs(testPos.x - player.position.x) < playerSafeRadius;

            bool tooCloseToOther = false;
            foreach (Vector3 pos in spawnedPositions) {
                if (Vector3.Distance(testPos, pos) < minDistanceBetweenDebris) {
                    tooCloseToOther = true;
                    break;
                }
            }

            if (!tooCloseToNPC && !tooCloseToPlayer && !tooCloseToOther) {
                randomPos = testPos;
                spawnedPositions.Add(randomPos);
                break;
            }
        }

        GameObject prefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Length)];

        GameObject obj = Instantiate(prefab, randomPos, Random.rotation);

        // Big readable spheres
        float scale = Random.Range(2f, 3.5f);
        obj.transform.localScale = Vector3.one * scale;
    }
}