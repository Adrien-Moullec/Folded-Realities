using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CollectiblesManager : MonoBehaviour {

    public static CollectiblesManager Instance;

    public int levelID = 0;

    public int normalCount = 0;
    public TMP_Text normalCountText;

    public GameObject specialCanvas;
    public Image[] puzzlePieces;
    private int specialCount = 0;

    public float pickupFloatSpeed = 2f;
    public float pickupRotationSpeed = 360f;
    public float destroyDelay = 0.6f;

    public AudioClip pickupSound;

    void Awake() {
        Instance = this;
    }

    void Start() {
        normalCount = 0;
        UpdateNormalUI();
        ResetPuzzleUI();
    }

    public void CollectNormal(GameObject obj) {
        normalCount++;
        UpdateNormalUI();

        if (CurrencyManager.Instance != null) {
            CurrencyManager.Instance.AddCoins(1);
        }

        StartCoroutine(PlayPickupEffect(obj));
    }

    public void CollectSpecial(GameObject obj) {
        if (specialCount < puzzlePieces.Length) {

            if (puzzlePieces[specialCount] != null) {
                puzzlePieces[specialCount].enabled = true;
            }

            specialCount++;

            if (CurrencyManager.Instance != null) {
                CurrencyManager.Instance.AddCoins(10);
            }

            if (specialCount == puzzlePieces.Length) {
                OnPuzzleCompleted();
            }
        }

        StartCoroutine(PlayPickupEffect(obj));
    }

    void OnPuzzleCompleted() {
        PlayerPrefs.SetInt("PuzzleComplete_Level_" + levelID, 1);
        PlayerPrefs.Save();
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