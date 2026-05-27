using UnityEngine;

public class TrashTriggerDrop : MonoBehaviour {

    [SerializeField] Rigidbody trashRB;

    [SerializeField] Renderer trashRenderer;

    [SerializeField] RectTransform watchOutText;

    [SerializeField] Transform cameraTransform;

    [SerializeField] float shakeDuration = 1f;

    [SerializeField] float textShakeAmount = 2f;

    [SerializeField] float cameraShakeAmount = 0.05f;

    [SerializeField] GameObject overHerePrompt;

    // Speed trash falls downward
    [SerializeField] float dropVelocity = 12f;

    Vector3 textStartPos;

    Vector3 cameraStartPos;

    bool activated;

    void Start() {

        // Hides warning visuals on startup
        trashRenderer.enabled = false;

        watchOutText.gameObject.SetActive(false);

        if (overHerePrompt != null)
            overHerePrompt.SetActive(false);

        textStartPos = watchOutText.localPosition;

        if (cameraTransform != null)
            cameraStartPos = cameraTransform.localPosition;

        // Freezes trash rigidbody until triggered
        if (trashRB != null) {

            trashRB.isKinematic = true;

            trashRB.useGravity = false;
        }
    }

    void OnTriggerEnter(Collider other) {

        // Activates falling trash event once
        if (!other.CompareTag("Player") || activated)
            return;

        activated = true;

        if (overHerePrompt != null)
            overHerePrompt.SetActive(false);

        trashRenderer.enabled = true;

        watchOutText.gameObject.SetActive(true);

        // Enables falling trash physics
        trashRB.isKinematic = false;

        trashRB.useGravity = true;

        trashRB.linearVelocity = Vector3.down * dropVelocity;

        trashRB.angularVelocity = Vector3.zero;

        StartCoroutine(Shake());
    }

    System.Collections.IEnumerator Shake() {

        float timer = 0f;

        // Shakes warning text and camera
        while (timer < shakeDuration) {

            float x = Mathf.Sin(Time.time * 60f) * textShakeAmount;

            float y = Mathf.Cos(Time.time * 60f) * textShakeAmount;

            watchOutText.localPosition =
                textStartPos + new Vector3(x, y, 0);

            if (cameraTransform != null) {

                float camX = Random.Range(
                    -cameraShakeAmount,
                    cameraShakeAmount
                );

                float camY = Random.Range(
                    -cameraShakeAmount,
                    cameraShakeAmount
                );

                cameraTransform.localPosition =
                    cameraStartPos + new Vector3(camX, camY, 0);
            }

            timer += Time.deltaTime;

            yield return null;
        }

        // Resets positions after shake
        watchOutText.localPosition = textStartPos;

        if (cameraTransform != null)
            cameraTransform.localPosition = cameraStartPos;

        watchOutText.gameObject.SetActive(false);
    }

    public void ResetTrigger() {

        activated = false;

        // Resets trash physics state
        if (trashRB != null) {

            trashRB.isKinematic = true;

            trashRB.useGravity = false;

            trashRB.linearVelocity = Vector3.zero;

            trashRB.angularVelocity = Vector3.zero;
        }

        if (trashRenderer != null)
            trashRenderer.enabled = false;
    }
}