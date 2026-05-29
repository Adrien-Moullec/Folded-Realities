using Unity.Mathematics;

using UnityEngine;

/// <summary>
/// A script for the falling gamemode damaging objects. They fly up and can hit the player.
/// </summary>
public class FallGameplayTriggerDamage : MonoBehaviour {
    [Tooltip("Damage done by the object.")]
    [SerializeField] int damage;
    [Tooltip("Freefall gamemode manager reference for controlling the object pool.")]
    FreefallGamemode freefallGamemode;
    [Tooltip("Current speed of this object.")]
    float speed;
    [Tooltip("Random point on a line.")]
    Vector3 randomLine;
    [Tooltip("Distance travelled in a frame.")]
    float distance;
    [Tooltip("Distance travelled in total.")]
    float totalDistance;

    /// <summary>
    /// Travel upwards over time. Despawn after a certain distance.
    /// </summary>
    void Update() {
        distance = Time.deltaTime * speed;
        transform.position += Vector3.up * distance;
        transform.Rotate(randomLine * speed * Time.deltaTime * 3);
        totalDistance += distance;
        if (totalDistance > 10) {
            freefallGamemode?.pooledObjects.Release(this);
        }
    }

    /// <summary>
    /// Try danage entity then despawn after hitting entity.
    /// </summary>
    /// <param name="other"> Collider of entity hit. </param>
    void OnTriggerEnter(Collider other) {
        if (!other.TryGetComponent(out IHealth ihealth)) return;
        ihealth.Damage(new AbilitySystem.EntityDamage(damage, null));
        freefallGamemode?.pooledObjects.Release(this);
    }

    /// <summary>
    /// Entity on spawn, setup values.
    /// </summary>
    /// <param name="freefallGamemode"> reference manager </param>
    /// <param name="speed"> set speed of object from manager </param>
    public void OnSpawn(FreefallGamemode freefallGamemode, float speed) {
        this.freefallGamemode = freefallGamemode;
        this.speed = speed;
        randomLine = UnityEngine.Random.onUnitSphere;
        totalDistance = 0;
    }
}
