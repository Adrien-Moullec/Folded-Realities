using UnityEngine;
using AbilitySystem;
using System;
using System.Collections;

public class MovingPlatformDown : MonoBehaviour {

    #region Boss References

    [Header("Boss References")]
    [SerializeField] PlatformSpawner spawner;

    [SerializeField] ShredderAnimatorManager shredderAnimatorManager;

    [SerializeField] Transform bossTarget;

    [SerializeField, Range(0, 1)]
    float suckInSpeed = 0.2f;

    [SerializeField]
    AnimationCurve suckInYEase;

    [SerializeField]
    AnimationCurve suckInPlanePositionEase;

    [SerializeField, Range(0, 1)]
    float targetSize = 0.2f;

    #endregion

    #region Variables

    // Distance before suction activates
    public float yDistanceToBossSuck = 4;

    float fallSpeed = 3f;

    bool suctionActive = false;

    Vector3 originalSize;

    #endregion

    void Awake() {

        originalSize = transform.localScale;
    }

    void Update() {

        if (suctionActive)
            return;

        // Falling movement
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Starts suction near boss
        if (!suctionActive && ((transform.position.y - bossTarget.position.y) < yDistanceToBossSuck))
            suctionActive = true;

        // Begins suction animation
        if (suctionActive && bossTarget != null)
            StartCoroutine(SuckAnim());
    }

    IEnumerator SuckAnim() {

        Vector3 startPos = transform.position;

        Vector3 endPos = bossTarget.position;

        float delta = 0;

        Vector3 pos;

        // Pulls platform into boss
        while (delta < 1) {

            delta += Time.deltaTime * suckInSpeed;

            pos = Vector3.Lerp(startPos, endPos, suckInYEase.Evaluate(delta));

            pos.y = Mathf.Lerp(startPos.y, endPos.y, suckInYEase.Evaluate(delta));

            transform.position = pos;

            // Shrinks platform during suction
            transform.localScale = Vector3.Lerp(
                originalSize,
                originalSize * targetSize,
                suckInYEase.Evaluate(delta)
            );

            yield return null;
        }

        spawner.pooledPlatforms.Release(this);
    }

    void OnTriggerEnter(Collider other) {

        // Damages valid targets
        if (!other.TryGetComponent(out IHealth iHealth))
            return;

        iHealth.Damage(new EntityDamage(10, null));

        spawner.pooledPlatforms.Release(this);
    }

    public void OnSpawnPlatform(PlatformSpawner spawner, float speed) {

        // Resets platform state on spawn
        suctionActive = false;

        this.spawner = spawner;

        fallSpeed = speed;

        transform.localScale = originalSize;
    }

    void OnDrawGizmos() {

        // Draws suction activation range
        Gizmos.DrawLine(
            bossTarget.transform.position,
            bossTarget.transform.position + Vector3.up * yDistanceToBossSuck
        );
    }
}