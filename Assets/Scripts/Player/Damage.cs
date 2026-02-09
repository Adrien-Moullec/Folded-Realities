using UnityEngine;

public class Damage : MonoBehaviour
{
    public enum DamageType
    {
        Hazard,
        Enemy,
        Environment
    }

    [Header("Damage Settings")]
    public int damageAmount = 10;
    public DamageType damageType = DamageType.Hazard;
    public float damageCooldown = 1f;

    float lastDamageTime;

    void OnCollisionEnter(Collision collision)
    {
        TryDealDamage(collision.gameObject);
    }

    void OnCollisionStay(Collision collision)
    {
        TryDealDamage(collision.gameObject);
    }

    void TryDealDamage(GameObject other)
    {
        if (Time.time - lastDamageTime < damageCooldown)
        {
            return;
        }

        iDamageable damageable = other.GetComponent<iDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
            lastDamageTime = Time.time;

            Debug.Log($"{damageType} dealt {damageAmount} damage");
        }
    }
}
