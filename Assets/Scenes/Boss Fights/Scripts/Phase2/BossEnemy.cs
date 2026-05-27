using UnityEngine;
using System.Collections;
using UnityEngine.Pool;
using System;

public class BossEnemy : MonoBehaviour {

    #region Movement

    [Header("Movement")]
    public float moveSpeed = 4f;

    public float minX = -8f;
    public float maxX = 8f;

    #endregion

    #region Projectile

    [Header("Projectile")]
    public BossProjectile projectilePrefab;

    public Transform shootPoint;

    public Transform player;

    #endregion

    #region Flash

    public float flashDuration = 0.5f;

    [HideInInspector]
    public bool isMove = false;

    #endregion

    ObjectPool<BossProjectile> pooledProjectiles;

    Renderer[] renderers;

    Material[] materials;

    bool movingRight = true;

    float centerX;

    Vector3 originalCentre;

    float lockedY;

    Vector3 pos;

    void Awake() {

        originalCentre = transform.position;

        lockedY = transform.position.y;

        // Creates projectile object pool
        projectilePrefab.gameObject.SetActive(false);

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

    void Start() {

        centerX = transform.position.x;

        // Stores material references for flash effects
        renderers = GetComponentsInChildren<Renderer>();

        materials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++) {

            materials[i] = renderers[i].material;

            materials[i].EnableKeyword("_EMISSION");
        }
    }

    void Update() {

        // Locks boss Y position
        pos = transform.position;

        pos.y = lockedY;

        transform.position = pos;

        if (isMove)
            Move();
    }

    #region Movement

    void Move() {

        float direction = movingRight ? 1f : -1f;

        pos = transform.position;

        pos.x += direction * moveSpeed * Time.deltaTime;

        float left = centerX + minX;

        float right = centerX + maxX;

        // Reverses direction at movement bounds
        if (pos.x >= right) {

            pos.x = right;

            movingRight = false;

        } else if (pos.x <= left) {

            pos.x = left;

            movingRight = true;
        }

        transform.position = pos;
    }

    public IEnumerator MoveToCentre() {

        // Returns boss to original position
        while (transform.position != originalCentre) {

            transform.position = Vector3.MoveTowards(
                transform.position,
                originalCentre,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    #endregion

    #region Projectiles

    void SpawnProjectile(BossProjectile projectile) {

        projectile.gameObject.SetActive(true);

        projectile.transform.position = shootPoint.transform.position;
    }

    public void DespawnProjectile(BossProjectile projectile) {

        projectile.gameObject.SetActive(false);
    }

    public void Shoot(float speed) {

        // Spawns projectile towards player
        if (projectilePrefab == null || shootPoint == null || player == null)
            return;

        pooledProjectiles.Get().OnSpawn(speed, player, this);
    }

    #endregion

    #region Visual Effects

    public IEnumerator FlashAngry(Color flashCol) {

        // Enables emission flash effect
        foreach (Material mat in materials) {

            mat.SetColor("_EmissionColor", flashCol * 6f);
        }

        yield return new WaitForSeconds(flashDuration);

        // Resets emission colour
        foreach (Material mat in materials) {

            mat.SetColor("_EmissionColor", Color.black);
        }
    }

    #endregion
}