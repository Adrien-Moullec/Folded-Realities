using AbilitySystem;

using UnityEngine;

public class EnemyDamage : MonoBehaviour {
    [SerializeField] int damage = 1;

    bool canDamage = true;

    void OnTriggerStay(Collider other) {
        if (!canDamage) {
            return;
        }

        PlayerHealthCanvas player =
            other.GetComponentInParent<PlayerHealthCanvas>();

        if (player != null) {
            Debug.Log("Player hit successfully");

            EntityDamage dmg =
                new EntityDamage(
                    damage,
                    null
                );

            if (other.TryGetComponent(out IHealth ihealth))
                ihealth.Damage(dmg);

            canDamage = false;
        }
    }

    void OnEnable() {
        Debug.Log("Hitbox ENABLED");

        canDamage = true;
    }

    void OnDisable() {
        Debug.Log("Hitbox DISABLED");
    }
}