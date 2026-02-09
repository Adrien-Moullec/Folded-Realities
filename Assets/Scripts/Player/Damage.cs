using UnityEngine;

public class Damage : MonoBehaviour
{
    public enum DamageType
    {
        Hazard,
        Enemy,
        Environment
    }

    public int damageAmount = 10;
    public DamageType damageType = DamageType.Hazard;
    public float damageCooldown = 1f;

    private float lastDamageTime;

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
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (Time.time - lastDamageTime < damageCooldown)
        {
            return;
        }

        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(damageAmount);
            lastDamageTime = Time.time;
        }
    }
}
