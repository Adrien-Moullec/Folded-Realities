using UnityEngine;
using System.Collections;

public class BossEnemy : MonoBehaviour {

    [Header("Movement")]
    public float moveSpeed = 4f;

    public float minX = -8f;

    public float maxX = 8f;

    [Header("Projectile")]
    public GameObject projectilePrefab;

    public Transform shootPoint;

    public Transform player;

    [Header("Flash")]
    public Color angryColor = Color.red;

    public float flashDuration = 0.5f;

    Renderer[] renderers;

    Material[] materials;

    bool movingRight = true;

    float centerX;

    void Start() {

        centerX =
            transform.position.x;

        renderers =
            GetComponentsInChildren<Renderer>();

        materials =
            new Material[
                renderers.Length
            ];

        for (int i = 0; i < renderers.Length; i++) {

            materials[i] =
                renderers[i].material;

            materials[i].EnableKeyword(
                "_EMISSION"
            );
        }
    }

    void Update() {

        Move();
    }

    void Move() {

        float direction =
            movingRight ? 1f : -1f;

        Vector3 pos =
            transform.position;

        pos.x +=
            direction *
            moveSpeed *
            Time.deltaTime;

        float left =
            centerX + minX;

        float right =
            centerX + maxX;

        if (pos.x >= right) {

            pos.x = right;

            movingRight = false;

        } else if (pos.x <= left) {

            pos.x = left;

            movingRight = true;
        }

        transform.position =
            pos;
    }

    public void Shoot(
        float speed
    ) {

        if (
            projectilePrefab == null ||
            shootPoint == null ||
            player == null
        ) {
            return;
        }

        GameObject proj =
            Instantiate(
                projectilePrefab,
                shootPoint.position,
                Quaternion.identity
            );

        BossProjectile bp =
            proj.GetComponent<BossProjectile>();

        if (bp != null) {

            bp.speed =
                speed;

            bp.SetTarget(
                player
            );
        }
    }

    public IEnumerator FlashAngry() {

        foreach (
            Material mat
            in materials
        ) {

            mat.SetColor(
                "_EmissionColor",
                angryColor * 6f
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
}