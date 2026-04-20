using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CollectiblesManager : MonoBehaviour {

    public static CollectiblesManager Instance;

    [Header("LEVEL SETTINGS")]
    public int levelID = 0; // set this per level in inspector

    [Header("Normal Collectibles")]
    public int normalCount = 0;
    public TMP_Text normalCountText;

    [Header("Special Collectibles")]
    public GameObject specialCanvas;
    public Image[] puzzlePieces;
    private int specialCount = 0;

    [Header("Pickup Effect Settings")]
    public float pickupFloatSpeed = 2f;
    public float pickupRotationSpeed = 360f;
    public float destroyDelay = 0.6f;

    [Header("Audio")]
    public AudioClip pickupSound;

    void Awake() {
        Instance = this;
    }

    void Start() {
        normalCount = 0;
        UpdateNormalUI();

        ResetPuzzleUI();

        if (specialCanvas != null) {
            specialCanvas.SetActive(false);
        }
    }

    // NORMAL COLLECTIBLE
    public void CollectNormal(GameObject obj) {
        normalCount++;
        UpdateNormalUI();

        if (CurrencyManager.Instance != null) {
            CurrencyManager.Instance.AddCoins(1);
        }

        StartCoroutine(PlayPickupEffect(obj));
    }

    // SPECIAL COLLECTIBLE
    public void CollectSpecial(GameObject obj) {

        // Activate canvas on first pickup
        if (specialCanvas != null && !specialCanvas.activeSelf) {
            specialCanvas.SetActive(true);
        }

        if (specialCount < puzzlePieces.Length) {

            puzzlePieces[specialCount].enabled = true;
            specialCount++;

            // +10 coins per special
            if (CurrencyManager.Instance != null) {
                CurrencyManager.Instance.AddCoins(10);
            }

            // Check if puzzle complete
            if (specialCount == puzzlePieces.Length) {
                OnPuzzleCompleted();
            }
        }

        StartCoroutine(PlayPickupEffect(obj));
    }

    // PUZZLE COMPLETE
    void OnPuzzleCompleted() {
        Debug.Log("Puzzle complete for level: " + levelID);

        // Save completion
        PlayerPrefs.SetInt("PuzzleComplete_Level_" + levelID, 1);
        PlayerPrefs.Save();

        // You can hook UI / sound here later
    }

    IEnumerator PlayPickupEffect(GameObject obj) {

        if (pickupSound != null) {
            AudioSource.PlayClipAtPoint(pickupSound, obj.transform.position);
        }

        Collider col = obj.GetComponent<Collider>();
        if (col != null) {
            col.enabled = false;
        }

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

        specialCount = 0;
    }

    public int GetCoinCount() {
        return normalCount;
    }
}