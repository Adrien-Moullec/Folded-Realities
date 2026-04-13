using UnityEngine;

public class BossEnemy : MonoBehaviour {
    public float moveSpeed = 4f;

    public float minX = -8f;
    public float maxX = 8f;

    public GameObject projectilePrefab;
    public Transform shootPoint;

    public float shootInterval = 2f;

    public Transform player;

    private float timer;
    private bool movingRight = true;

    void Update() {
        Move();

        timer += Time.deltaTime;

        if (timer >= shootInterval) {
            ShootAtPlayerPlatform();
            timer = 0f;
        }
    }
    float centerX;

    void Start() {
        centerX = transform.position.x;
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

    void ShootAtPlayerPlatform() {
        if (projectilePrefab == null || shootPoint == null || player == null)
            return;

        RaycastHit hit;

        if (Physics.Raycast(player.position, Vector3.down, out hit, 20f)) {
            Debug.DrawRay(player.position, Vector3.down * 20f, Color.red, 1f);

            if (hit.collider.CompareTag("Platform")) {
                GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

                BossProjectile bp = proj.GetComponent<BossProjectile>();

                if (bp != null) {
                    bp.SetTarget(hit.collider.transform.position + Vector3.up * 0.5f);
                }
            }
        }
    }
}