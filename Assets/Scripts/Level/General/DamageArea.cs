using UnityEngine;
using System.Collections;

public class DamageArea : MonoBehaviour {

    public int damageAmount = 1;

    // Delay between damage ticks
    public float damageCooldown = 1f;

    public float flashDuration = 0.2f;

    public Color flashColor = Color.red;

    public float slowStrength = 3f;

    public float lingerDuration = 2f;

    float lastDamageTime;

    Renderer objectRenderer;

    Color originalColor;

    bool isFlashing = false;

    Coroutine slowCoroutine;

    void Start() {

        objectRenderer = GetComponent<Renderer>();

        // Stores original material colour
        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;
    }

    void OnTriggerEnter(Collider other) {

        if (!other.CompareTag("Player"))
            return;

        ApplyEffects(other.gameObject);
    }

    void OnTriggerStay(Collider other) {

        if (!other.CompareTag("Player"))
            return;

        ApplyEffects(other.gameObject);
    }

    void OnTriggerExit(Collider other) {

        if (!other.CompareTag("Player"))
            return;

        // Applies lingering slow effect after exit
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        slowCoroutine = StartCoroutine(
            ApplyLingerSlow(other.gameObject)
        );
    }

    void ApplyEffects(GameObject player) {

        // Prevents rapid repeated damage
        if (Time.time - lastDamageTime < damageCooldown)
            return;

        lastDamageTime = Time.time;

        // Starts damage flash effect
        if (!isFlashing)
            StartCoroutine(FlashRed());

        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        slowCoroutine = StartCoroutine(SlowPlayer(player));
    }

    IEnumerator SlowPlayer(GameObject player) {

        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller == null)
            yield break;

        // Applies movement slowdown
        while (true) {

            Vector3 velocity = controller.velocity;

            Vector3 horizontal =
                new Vector3(velocity.x, 0f, velocity.z);

            if (horizontal.magnitude > 0.1f) {

                Vector3 slowMove =
                    -horizontal.normalized
                    * slowStrength
                    * Time.deltaTime;

                if (slowMove.magnitude > horizontal.magnitude)
                    slowMove = -horizontal;

                controller.Move(slowMove);
            }

            yield return null;
        }
    }

    IEnumerator ApplyLingerSlow(GameObject player) {

        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller == null)
            yield break;

        float timer = 0f;

        // Continues slowdown briefly after leaving area
        while (timer < lingerDuration) {

            Vector3 velocity = controller.velocity;

            Vector3 horizontal =
                new Vector3(velocity.x, 0f, velocity.z);

            if (horizontal.magnitude > 0.1f) {

                Vector3 slowMove =
                    -horizontal.normalized
                    * slowStrength
                    * Time.deltaTime;

                if (slowMove.magnitude > horizontal.magnitude)
                    slowMove = -horizontal;

                controller.Move(slowMove);
            }

            timer += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator FlashRed() {

        if (objectRenderer == null)
            yield break;

        isFlashing = true;

        // Flashes object material red
        objectRenderer.material.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        objectRenderer.material.color = originalColor;

        isFlashing = false;
    }
}