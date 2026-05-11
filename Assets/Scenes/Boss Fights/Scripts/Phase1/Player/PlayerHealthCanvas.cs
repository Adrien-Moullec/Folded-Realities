using AbilitySystem;

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthCanvas : MonoBehaviour {//, IHealth {
    /*[Header("Health")]
    [SerializeField] int maxHealth = 100;

    int currentHealth;*/

    [Header("Heart UI")]
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite darkHeart;


    void Awake() {
        UpdateHearts(100);
    }

    public void UpdateHearts(int currentHealth) {
        for (int i = 0; i < hearts.Length; i++) {
            int healthPerHeart = 20;

            int heartHealth =
                currentHealth - (i * healthPerHeart);

            if (heartHealth >= 20) {
                hearts[i].sprite =
                    fullHeart;
            } else if (heartHealth >= 10) {
                hearts[i].sprite =
                    halfHeart;
            } else {
                hearts[i].sprite =
                    darkHeart;
            }
        }

        Debug.Log(
            "Hearts updated"
        );
    }


    /*
    bool invincible;

    CharacterController controller;


    void Start() {
        controller =
            GetComponent<CharacterController>();

        currentHealth = maxHealth;

        Debug.Log(
            "Player health initialized: "
            + currentHealth
        );

    }*/
    /*public void Damage(EntityDamage damage) {
        Debug.Log(
            "Damage received: "
            + damage.amount
        );

        if (invincible) {
            Debug.Log(
                "Player currently invincible"
            );

            return;
        }

        currentHealth -= Mathf.RoundToInt(
            damage.amount
        );

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth
        );

        Debug.Log(
            "Current health: "
            + currentHealth
        );

        UpdateHearts();

        if (currentHealth <= 0) {
            Debug.Log(
                "Health reached zero"
            );

            Die();
        }

        StartCoroutine(
            InvincibilityFrames()
        );
    }

    public void Heal(EntityDamage heal) {
        currentHealth += Mathf.RoundToInt(
            heal.amount
        );

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth
        );

        Debug.Log(
            "Healed player. Current health: "
            + currentHealth
        );

        UpdateHearts();
    }*/


    /*
        public void Die() {
            Debug.Log(
                "Player died"
            );

            Respawn();
        }*/



    /*
    if (controller != null) {
        controller.enabled = false;
    }

    // controller?.enabled = false;


    transform.position =
        respawnPosition;

    if (controller != null) {
        controller.enabled = true;
    }*/
    //}
}