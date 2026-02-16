using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    public int damageAmount = 1;   // IMPORTANT: 1 heart per hit
    public float damageCooldown = 1f;

    [Header("Flash Settings")]
    public float flashDuration = 0.2f;
    public Color flashColor = Color.red;

    float lastDamageTime;

    Renderer objectRenderer;
    Color originalColor;
    bool isFlashing = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        TryDealDamage(collision.gameObject);
    }

  

    void TryDealDamage(GameObject other)
    {
        if (Time.time - lastDamageTime < damageCooldown)
        {
            return;
        }

        IHealth health = other.GetComponentInParent<IHealth>();

        if (health != null)
        {
            health.TakeDamage(damageAmount);
            lastDamageTime = Time.time;

            if (!isFlashing)
            {
                StartCoroutine(FlashRed());
            }
        }
    }

    IEnumerator FlashRed()
    {
        if (objectRenderer == null)
        {
            yield break;
        }

        isFlashing = true;

        objectRenderer.material.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        objectRenderer.material.color = originalColor;

        isFlashing = false;
    }
}
