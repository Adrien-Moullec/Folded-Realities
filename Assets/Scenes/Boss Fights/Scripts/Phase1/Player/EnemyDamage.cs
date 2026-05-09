using AbilitySystem;

using UnityEngine;

public class EnemyDamage : MonoBehaviour {
    [SerializeField] int damage = 1;

    bool canDamage = true;

    void OnTriggerStay(Collider other) {
        if (!canDamage) {
            return;
        }

        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player != null) {
            Debug.Log("Player hit successfully");

            EntityDamage dmg =
                new EntityDamage(
                    damage,
                    null
                );

            player.Damage(dmg);

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