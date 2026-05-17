using UnityEngine;

public class BreakableBlock : MonoBehaviour {
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
}