using UnityEngine;

public class BossBounce : MonoBehaviour {
    public float bounceForce = 8f;
    public float flashTime = 0.2f;

    private Renderer rend;
    private Color originalColor;

    void Start() {
        rend = GetComponent<Renderer>();

        if (rend != null) {
            originalColor = rend.material.color;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.CompareTag("Player")) {
            return;
        }

        CharacterController cc = collision.gameObject.GetComponent<CharacterController>();

        if (cc != null) {
            cc.Move(Vector3.up * bounceForce * Time.deltaTime);
        }

        StartCoroutine(FlashRed());
    }

    System.Collections.IEnumerator FlashRed() {
        if (rend != null) {
            rend.material.color = Color.red;
        }

        yield return new WaitForSeconds(flashTime);

        if (rend != null) {
            rend.material.color = originalColor;
        }
    }
}