using Unity.Mathematics;

using UnityEngine;

public class FallGameplayTriggerDamage : MonoBehaviour {
    [SerializeField] int damage;
    FreefallGamemode freefallGamemode;
    float speed;
    Vector3 randomLine;

    float distance;
    float totalDistance;
    void Update() {
        distance = Time.deltaTime * speed;
        transform.position += Vector3.up * distance;
        transform.Rotate(randomLine * speed * Time.deltaTime * 3);
        totalDistance += distance;
        if (totalDistance > 10) {
            freefallGamemode?.pooledObjects.Release(this);
        }

    }
    void OnTriggerEnter(Collider other) {
        if (!other.TryGetComponent(out IHealth ihealth)) return;
        ihealth.Damage(new AbilitySystem.EntityDamage(damage, null));
        freefallGamemode?.pooledObjects.Release(this);
    }
    public void OnSpawn(FreefallGamemode freefallGamemode, float speed) {
        this.freefallGamemode = freefallGamemode;
        this.speed = speed;
        randomLine = UnityEngine.Random.onUnitSphere;
        totalDistance = 0;
    }
}
