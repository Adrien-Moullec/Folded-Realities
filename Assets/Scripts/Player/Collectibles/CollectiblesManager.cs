using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CollectiblesManager : MonoBehaviour {
    [Header("Normal Collectibles")]
    public int normalCount = 0;
    public TMP_Text normalCountText;

    [Header("Special Collectibles")]
    public Image[] puzzlePieces;
    private int specialCount = 0;

    [Header("Pickup Effect Settings")]
    public float pickupFloatSpeed = 2f;
    public float pickupRotationSpeed = 360f;
    public float destroyDelay = 0.6f;

    [Header("Audio")]
    public AudioClip pickupSound;

    void Start() {
        UpdateNormalUI();
        ResetPuzzleUI();
    }

    // Called when star is collected
    public void CollectNormal(GameObject obj) {
        normalCount++;
        UpdateNormalUI();

        StartCoroutine(PlayPickupEffect(obj));
    }

    // Called when puzzle piece is collected
    public void CollectSpecial(GameObject obj) {
        if (specialCount < puzzlePieces.Length) {
            puzzlePieces[specialCount].enabled = true;
            specialCount++;
        }

        StartCoroutine(PlayPickupEffect(obj));
    }

    IEnumerator PlayPickupEffect(GameObject obj) {
        // Play pickup sound
        if (pickupSound != null) {
            AudioSource.PlayClipAtPoint(pickupSound, obj.transform.position);
        }

        // Disable collider
        Collider col = obj.GetComponent<Collider>();
        if (col != null) {
            col.enabled = false;
        }

        // Stop idle floating
        CollectibleIdle idle = obj.GetComponent<CollectibleIdle>();
        if (idle != null) {
            idle.enabled = false;
        }

        float timer = 0f;

        while (timer < destroyDelay) {
            obj.transform.position += Vector3.up * pickupFloatSpeed * Time.deltaTime;
            obj.transform.Rotate(Vector3.up * pickupRotationSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }

    void UpdateNormalUI() {
        if (normalCountText != null) {
            normalCountText.text = normalCount.ToString();
        }
    }

    void ResetPuzzleUI() {
        for (int i = 0; i < puzzlePieces.Length; i++) {
            if (puzzlePieces[i] != null) {
                puzzlePieces[i].enabled = false;
            }
        }
    }
}