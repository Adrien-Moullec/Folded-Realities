using UnityEngine;

using AbilitySystem;

using System;
using System.Collections;

public class MovingPlatformDown : MonoBehaviour {

    [Header("Boss References")]
    [SerializeField] PlatformSpawner spawner;
    [SerializeField] ShredderAnimatorManager shredderAnimatorManager;
    [SerializeField] Transform bossTarget;
    [SerializeField, Range(0, 1)] float suckInSpeed = 0.2f;
    [SerializeField] AnimationCurve suckInYEase;
    [SerializeField] AnimationCurve suckInPlanePositionEase;
    [SerializeField, Range(0, 1)] float targetSize = 0.2f;

    // Private
    public float yDistanceToBossSuck = 4;
    float fallSpeed = 3f;
    bool suctionActive = false;
    Vector3 originalSize;

    void Awake() {
        originalSize = transform.localScale;
    }


    void Update() {
        if (suctionActive) return;

        // normal falling
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // only start suction AFTER passing player
        if (!suctionActive && ((transform.position.y - bossTarget.position.y) < yDistanceToBossSuck))
            suctionActive = true;

        // suck toward boss
        if (suctionActive && bossTarget != null)
            StartCoroutine(SuckAnim());
    }

    IEnumerator SuckAnim() {
        Vector3 startPos = transform.position;
        Vector3 endPos = bossTarget.position;
        float delta = 0;
        Vector3 pos;
        while (delta < 1) {
            delta += Time.deltaTime * suckInSpeed;
            pos = Vector3.Lerp(startPos, endPos, suckInYEase.Evaluate(delta));
            pos.y = Mathf.Lerp(startPos.y, endPos.y, suckInYEase.Evaluate(delta));
            transform.position = pos;
            transform.localScale = Vector3.Lerp(originalSize, originalSize * targetSize, suckInYEase.Evaluate(delta));
            yield return null;
        }
        spawner.pooledPlatforms.Release(this);
    }

    void OnTriggerEnter(Collider other) {
        if (!other.TryGetComponent(out IHealth iHealth)) return;
        iHealth.Damage(new EntityDamage(10, null));
        spawner.pooledPlatforms.Release(this);
    }

    public void OnSpawnPlatform(PlatformSpawner spawner, float speed) {
        suctionActive = false;
        this.spawner = spawner;
        fallSpeed = speed;
        transform.localScale = originalSize;
    }

    void OnDrawGizmos() {
        Gizmos.DrawLine(bossTarget.transform.position, bossTarget.transform.position + Vector3.up * yDistanceToBossSuck);
    }
}