using AbilitySystem;

using UnityEngine;

public class BreakableBlock : MonoBehaviour, IHealth {
    public GameObject breakEffect;

    public AudioSource breakSound;

    public void Break() {
        if (breakEffect != null) {
            Instantiate(
                breakEffect,
                transform.position,
                Quaternion.identity
            );
        }

        if (breakSound != null) {
            AudioSource sound =
                Instantiate(
                    breakSound,
                    transform.position,
                    Quaternion.identity
                );

            sound.Play();

            Destroy(
                sound.gameObject,
                sound.clip.length
            );
        }

        Destroy(gameObject);
    }

    public void Damage(EntityDamage damage) {
        Break();
    }

    public void Die() {
        throw new System.NotImplementedException();
    }

    public void Heal(EntityDamage heal) {
        throw new System.NotImplementedException();
    }

    public void SetMaxHealth() {
        throw new System.NotImplementedException();
    }
}