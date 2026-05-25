using UnityEngine;

using AbilitySystem;

using System;

public class BossProjectile : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] float speed = 8f;

    [SerializeField] float lifeTime = 10f;

    [SerializeField] float overshootAmount = 8f;

    [Header("Damage")]
    [SerializeField] int damageAmount = 10;

    BossEnemy bossEnemy;

    Vector3 moveDirection;
    float DistanceMoved;

    public void SetTarget(Transform target) {
        if (target != null)
            moveDirection = (target.position - transform.position).normalized;
    }

    void Update() {
        float Distance = speed * Time.deltaTime;
        transform.position += moveDirection * Distance;
        DistanceMoved += Distance;

        if (DistanceMoved > overshootAmount)
            bossEnemy.DespawnProjectile(this);
    }


    void OnTriggerEnter(Collider other) {
        if (!other.TryGetComponent(out IHealth iHealth)) return;
        iHealth.Damage(new EntityDamage(damageAmount, null));
        bossEnemy.DespawnProjectile(this);
    }

    internal void OnSpawn(float speed, Transform player, BossEnemy bossEnemy) {
        this.speed = speed;
        DistanceMoved = 0;
        SetTarget(player);
        this.bossEnemy = bossEnemy;
    }
}