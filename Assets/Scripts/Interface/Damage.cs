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

        IHealth health = other.GetComponent<IHealth>();

        if (health != null)
        {
            health.TakeDamage(damageAmount);
            lastDamageTime = Time.time;
        }
    }
}
