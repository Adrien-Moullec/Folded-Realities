using UnityEngine;

public class DebrisSpawner : MonoBehaviour {
    [Header("Prefabs")]
    public GameObject[] debrisPrefabs;

    [Header("References")]
    public Transform npc;

    [Header("Spawn Settings")]
    public int spawnCount = 50;
    public Vector3 areaSize = new Vector3(20, 50, 20);

    [Header("NPC Safe Zone")]
    public float npcSafeRadius = 3f;

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
}