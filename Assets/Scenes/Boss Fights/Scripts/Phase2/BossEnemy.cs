using UnityEngine;

using System.Collections;
using UnityEngine.Pool;
using System;

public class BossEnemy : MonoBehaviour {

    [Header("Movement")]
    public float moveSpeed = 4f;

    public float minX = -8f;

    public float maxX = 8f;

    [Header("Projectile")]
    public BossProjectile projectilePrefab;

    public Transform shootPoint;

    public Transform player;
    public float flashDuration = 0.5f;
    [HideInInspector] public bool isMove = false;

    ObjectPool<BossProjectile> pooledProjectiles;
    Renderer[] renderers;
    Material[] materials;
    bool movingRight = true;
    float centerX;
    Vector3 originalCentre;

    private float lockedY;
    Vector3 pos;

    void Awake() {
        originalCentre = transform.position;
        projectilePrefab.gameObject.SetActive(false);
        lockedY = transform.position.y;
        pooledProjectiles = new ObjectPool<BossProjectile>(
            createFunc: () => Instantiate(projectilePrefab),
            actionOnGet: projectile => SpawnProjectile(projectile),
            actionOnRelease: projectile => DespawnProjectile(projectile),
            actionOnDestroy: projectile => Destroy(projectile.gameObject),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100
        );
    }
    void SpawnProjectile(BossProjectile projectile) {
        projectile.gameObject.SetActive(true);
        projectile.transform.position = shootPoint.transform.position;
    }
    public void DespawnProjectile(BossProjectile projectile) {
        projectile.gameObject.SetActive(false);
    }
    void Start() {

        centerX = transform.position.x;
        renderers = GetComponentsInChildren<Renderer>();
        materials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++) {
            materials[i] = renderers[i].material;
            materials[i].EnableKeyword("_EMISSION");
        }
    }

    void Update() {
        pos = transform.position;
        pos.y = lockedY;
        transform.position = pos;
        if (isMove) Move();
    }

    void Move() {

        float direction = movingRight ? 1f : -1f;

        pos = transform.position;

        pos.x += direction *
            moveSpeed *
            Time.deltaTime;

        float left = centerX + minX;

        float right = centerX + maxX;

        if (pos.x >= right) {
            pos.x = right;
            movingRight = false;
        } else if (pos.x <= left) {
            pos.x = left;
            movingRight = true;
        }

        transform.position = pos;
    }

    public void Shoot(float speed) {

        if (projectilePrefab == null || shootPoint == null || player == null)
            return;
        pooledProjectiles.Get().OnSpawn(speed, player, this);
    }

    public IEnumerator FlashAngry(Color flashCol) {

        foreach (
            Material mat
            in materials
        ) {

            mat.SetColor(
                "_EmissionColor",
                flashCol * 6f
            );
        }

        yield return new WaitForSeconds(
            flashDuration
        );

        foreach (
            Material mat
            in materials
        ) {

            mat.SetColor(
                "_EmissionColor",
                Color.black
            );
        }
    }

    public IEnumerator MoveToCentre() {
        while (transform.position != originalCentre) {
            transform.position = Vector3.MoveTowards(transform.position, originalCentre, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}