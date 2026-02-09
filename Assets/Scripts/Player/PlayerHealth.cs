using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, iDamageable, IHealable, IKillable, IHasHealth
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    public float MaxHealth => maxHealth;

    public float CurrentHealth { get; private set; }

    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    void Awake()
    {
        CurrentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = CurrentHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        if (CurrentHealth <= 0)
        {
            return;
        }

        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        UpdateUI();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (CurrentHealth <= 0)
        {
            return;
        }

        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        UpdateUI();
    }

    public void Die()
    {
        Debug.Log("Player died");
        // Disable movement, play animation, trigger event, etc.
    }

    private void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = CurrentHealth;
        }
    }
}

internal interface IHasHealth
{
}

internal interface IKillable
{
}

internal interface IHealable
{
}

