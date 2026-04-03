using AbilitySystem;

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth {
    [Header("Health Settings")]
    [SerializeField] int maxHealth = 5;
    int currentHealth;

    [Header("Heart UI")]
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    // Proper interface implementation (NO exceptions)
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    void Start() {
        currentHealth = maxHealth;
        UpdateHearts();
    }

    public void Damage(float amount, EntityBody entityBody = null) {
        currentHealth -= Mathf.RoundToInt(amount);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHearts();

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Heal(float amount, EntityBody entityBody = null) {
        currentHealth += Mathf.RoundToInt(amount);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHearts();
    }

    void UpdateHearts() {
        if (hearts == null || hearts.Length == 0) {
            return;
        }

        for (int i = 0; i < hearts.Length; i++) {
            if (hearts[i] == null) {
                continue;
            }

            hearts[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
        }
    }

    public void Die() {
        Debug.Log("Player died");


        Collider col = GetComponent<Collider>();
        if (col != null) {
            col.enabled = false;
        }
    }
}
