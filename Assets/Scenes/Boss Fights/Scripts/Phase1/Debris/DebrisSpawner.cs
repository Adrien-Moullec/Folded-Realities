using UnityEngine;

public class DebrisSpawnerAndWind : MonoBehaviour {
    [Header("Debris")]
    public GameObject[] debrisPrefabs;

    [Header("Wind")]
    public GameObject windPrefab;
    public int windCount = 20;

    [Header("References")]
    public Transform npc;

    [Header("Spawn Settings")]
    public int spawnCount = 50;
    public Vector3 areaSize = new Vector3(20, 50, 20);

    [Header("NPC Safe Zone")]
    public float npcSafeRadius = 3f;

    void Start() {
        // Spawn debris
        for (int i = 0; i < spawnCount; i++) {
            SpawnDebris(false);
        }

        for (int i = 0; i < spawnCount; i++) {
            SpawnDebris(true);
        }

        for (int i = 0; i < windCount; i++) {
            SpawnWind();
        }
    }

    void SpawnDebris(bool bottomLayer) {
        Vector3 randomPos = transform.position;

        for (int i = 0; i < 10; i++) {
            float yMin = bottomLayer ? -areaSize.y / 2f : -areaSize.y / 4f;
            float yMax = bottomLayer ? -areaSize.y / 4f : areaSize.y / 2f;

            Vector3 testPos = transform.position + new Vector3(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                Random.Range(yMin, yMax),
                Random.Range(-areaSize.z / 2f, areaSize.z / 2f)
            );

            bool tooCloseToNPC = npc != null &&
                Vector3.Distance(testPos, npc.position) < npcSafeRadius;

            if (!tooCloseToNPC) {
                randomPos = testPos;
                break;
            }
        }

        GameObject prefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Length)];

        GameObject obj = Instantiate(prefab, randomPos, Random.rotation);

        float scale = Random.Range(2f, 3.5f);
        obj.transform.localScale = Vector3.one * scale;
    }


    void SpawnWind() {
        if (windPrefab == null) {
            return;
        }

        float halfWidth = areaSize.x / 2f;

        // Spawn near LEFT or RIGHT edges
        float side = Random.value > 0.5f ? -1f : 1f;

        float x = transform.position.x + side * (halfWidth - 1f);

        float y = transform.position.y + Random.Range(-areaSize.y / 2f, areaSize.y / 2f);

        float z = transform.position.z + Random.Range(-areaSize.z / 2f, areaSize.z / 2f);

        Vector3 pos = new Vector3(x, y, z);

        Instantiate(windPrefab, pos, Quaternion.identity);
    }
}