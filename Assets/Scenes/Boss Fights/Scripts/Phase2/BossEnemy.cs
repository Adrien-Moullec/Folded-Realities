using UnityEngine;
using System.Collections;

public class BossEnemy : MonoBehaviour {
    public float moveSpeed = 4f;

    public float minX = -8f;
    public float maxX = 8f;

    public GameObject projectilePrefab;
    public Transform shootPoint;

    public float shootInterval = 2f;
    public float shootPauseTime = 1.2f;

    public Transform player;

    private float timer;
    private bool movingRight = true;
    private bool isAttacking = false;

    float centerX;

    void Start() {
        centerX = transform.position.x;
        Debug.Log("Boss started");
    }

    void Update() {
        if (!isAttacking) {
            Move();

            timer += Time.deltaTime;

            if (timer >= shootInterval) {
                StartCoroutine(AttackRoutine());
                timer = 0f;
            }
        }
    }

    void Move() {
        float direction = movingRight ? 1f : -1f;

        Vector3 pos = transform.position;
        pos.x += direction * moveSpeed * Time.deltaTime;

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

    IEnumerator AttackRoutine() {
        isAttacking = true;

        Debug.Log("Boss attacking");

        yield return new WaitForSeconds(shootPauseTime);

        ShootAtNearestPlatform();

        yield return new WaitForSeconds(0.3f);

        isAttacking = false;
    }

    void ShootAtNearestPlatform() {
        if (projectilePrefab == null || shootPoint == null || player == null) {
            Debug.LogError("Missing references");
            return;
        }

        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");

        if (platforms.Length == 0) {
            Debug.LogError("No platforms found");
            return;
        }

        Transform closestPlatform = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject p in platforms) {
            float dist = Vector3.Distance(player.position, p.transform.position);

            if (dist < closestDistance) {
                closestDistance = dist;
                closestPlatform = p.transform;
            }
        }

        if (closestPlatform == null) {
            Debug.LogWarning("No platform selected");
            return;
        }

        Debug.Log("Targeting platform: " + closestPlatform.name);

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        BossProjectile bp = proj.GetComponent<BossProjectile>();

        if (bp != null) {
            bp.SetTarget(closestPlatform);
        }
    }
}