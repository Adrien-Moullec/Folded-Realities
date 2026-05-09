using AbilitySystem;

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth {
    [Header("Health")]
    [SerializeField] int maxHealth = 100;

    int currentHealth;

    [Header("Heart UI")]
    [SerializeField] Image[] hearts;

    [SerializeField] Sprite fullHeart;

    [SerializeField] Sprite halfHeart;

    [SerializeField] Sprite darkHeart;

    [Header("Damage Settings")]
    [SerializeField] float invincibilityTime = 1f;

    [Header("Respawn")]
    [SerializeField] Transform startPoint;

    bool invincible;

    CharacterController controller;

    public float CurrentHealth => currentHealth;

    public float MaxHealth => maxHealth;

    void Start() {
        controller =
            GetComponent<CharacterController>();

        currentHealth = maxHealth;

        Debug.Log(
            "Player health initialized: "
            + currentHealth
        );

        UpdateHearts();
    }

    public void Damage(EntityDamage damage) {
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
    }

    void UpdateHearts() {
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

    System.Collections.IEnumerator InvincibilityFrames() {
        invincible = true;

        Debug.Log(
            "Invincibility ON"
        );

        yield return new WaitForSeconds(
            invincibilityTime
        );

        invincible = false;

        Debug.Log(
            "Invincibility OFF"
        );
    }

    public void Die() {
        Debug.Log(
            "Player died"
        );

        Respawn();
    }

    void Respawn() {
        currentHealth = maxHealth;

        Debug.Log(
            "Respawning player"
        );

        UpdateHearts();

        Vector3 respawnPosition =
            startPoint.position;

        if (
            CheckpointManager.Instance != null
            && CheckpointManager.Instance.HasCheckpoint()
        ) {
            respawnPosition =
                CheckpointManager.Instance
                .GetCheckpointPosition();

            Debug.Log(
                "Using checkpoint position: "
                + respawnPosition
            );
        } else {
            Debug.Log(
                "Using start position: "
                + respawnPosition
            );
        }

        if (controller != null) {
            controller.enabled = false;
        }

        transform.position =
            respawnPosition;

        if (controller != null) {
            controller.enabled = true;
        }
    }
}