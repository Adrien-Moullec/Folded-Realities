using Unity.Mathematics;

using UnityEngine;

public class FallGameplayTriggerDamage : MonoBehaviour {
    [SerializeField] int damage;
    FreefallGamemode freefallGamemode;
    float speed;
    Vector3 randomLine;
    void Update() {
        transform.position += Vector3.up * Time.deltaTime * speed;
        transform.Rotate(randomLine * speed * Time.deltaTime);

    }
    void OnTriggerEnter(Collider other) {
        if (!other.TryGetComponent(out IHealth ihealth)) return;
        ihealth.Damage(new AbilitySystem.EntityDamage(damage, null));
        freefallGamemode?.Despawn(this);
    }
    public void OnSpawn(FreefallGamemode freefallGamemode, Vector3 pos, float speed) {
        transform.position = pos;
        this.freefallGamemode = freefallGamemode;
        this.speed = speed;
        randomLine = UnityEngine.Random.onUnitSphere;
    }
}
