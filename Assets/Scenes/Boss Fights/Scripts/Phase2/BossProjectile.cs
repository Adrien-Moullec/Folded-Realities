using UnityEngine;
using AbilitySystem;
using System;

public class BossProjectile : MonoBehaviour {

    #region Movement

    [Header("Movement")]
    [SerializeField] float speed = 8f;

    [SerializeField] float lifeTime = 10f;

    // Distance projectile travels before despawning
    [SerializeField] float overshootAmount = 8f;

    #endregion

    #region Damage

    [Header("Damage")]
    [SerializeField] int damageAmount = 10;

    #endregion

    BossEnemy bossEnemy;

    Vector3 moveDirection;

    float DistanceMoved;

    public void SetTarget(Transform target) {

        // Calculates projectile direction
        if (target != null)
            moveDirection = (target.position - transform.position).normalized;
    }

    void Update() {

        // Moves projectile forward
        float Distance = speed * Time.deltaTime;

        transform.position += moveDirection * Distance;

        DistanceMoved += Distance;

        // Despawns projectile after max distance
        if (DistanceMoved > overshootAmount)
            bossEnemy.DespawnProjectile(this);
    }

    void OnTriggerEnter(Collider other) {

        // Damages valid health targets
        if (!other.TryGetComponent(out IHealth iHealth))
            return;

        iHealth.Damage(new EntityDamage(damageAmount, null));

        bossEnemy.DespawnProjectile(this);
    }

    internal void OnSpawn(float speed, Transform player, BossEnemy bossEnemy) {

        // Resets projectile values on spawn
        this.speed = speed;

        DistanceMoved = 0;

        SetTarget(player);

        this.bossEnemy = bossEnemy;
    }
}