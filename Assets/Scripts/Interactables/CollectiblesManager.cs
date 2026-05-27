using UnityEngine;
using UnityEngine.UI;

using TMPro;

using AbilitySystem;

public class CollectiblesManager : MonoBehaviour {

    public static CollectiblesManager Instance;

    public int levelID = 0;

    public int normalCount = 0;

    public TMP_Text normalCountText;

    public GameObject specialCanvas;

    public Image[] puzzlePieces;

    private int specialCount = 0;

    public AudioClip pickupSound;
    // Health Pickup
    [Header("Health Pickup")]
    public int healAmount = 20;

    void Awake() {

        Instance = this;
    }

    void Start() {

        normalCount = 0;

        UpdateNormalUI();

        ResetPuzzleUI();

        if (
            specialCanvas != null
        ) {

            specialCanvas.SetActive(
                true
            );
        }
    }
    #region Normal Collectibles
    // Awards player coins
    public void CollectNormal(
        GameObject obj
    ) {

        normalCount++;

        UpdateNormalUI();

        if (
            CurrencyManager.Instance
            != null
        ) {

            CurrencyManager.Instance.AddCoins(
                1
            );
        }

        PlayPickupSound(
            obj.transform.position
        );

        Destroy(
            obj
        );
    }
    #endregion

    #region Special Collect
    public void CollectSpecial(
        GameObject obj
    ) {
        // Prevents overflow errors
        if (
            specialCount
            >=
            puzzlePieces.Length
        ) {
            return;
        }

        if (
            puzzlePieces[specialCount]
            == null
        ) {
            return;
        }
        // Checks whether puzzle is complete
        puzzlePieces[specialCount]
            .gameObject
            .SetActive(
                true
            );

        specialCount++;

        if (
            CurrencyManager.Instance
            != null
        ) {

            CurrencyManager.Instance.AddCoins(
                10
            );
        }

        if (
            specialCount
            ==
            puzzlePieces.Length
        ) {

            OnPuzzleCompleted();
        }

        PlayPickupSound(
            obj.transform.position
        );

        Destroy(
            obj
        );
    }

    #endregion

    #region Health Collectible
    // Gets player health controller
    public void CollectHealth(
        GameObject obj,
        GameObject player
    ) {

        PlayerAbilityController health =
            player.GetComponent<
                PlayerAbilityController
            >();
        // Restores player health
        if (
            health != null
        ) {

            health.SetMaxHealth();
        }

        PlayPickupSound(
            obj.transform.position
        );

        Destroy(
            obj
        );
    }

    void PlayPickupSound(
        Vector3 pos
    ) {

        if (
            pickupSound != null
        ) {

            AudioSource.PlayClipAtPoint(
                pickupSound,
                pos
            );
        }
    }
    #endregion
    void OnPuzzleCompleted() {

        GameplaySystem.SetInt(PrefInt.PuzzleComplete, 1);
        GameplaySystem.SaveSettings();
    }

    void UpdateNormalUI() {

        if (
            normalCountText != null
        ) {

            normalCountText.text =
                normalCount.ToString();
        }
    }

    void ResetPuzzleUI() {
        // Hides all puzzle piece UI elements
        for (
            int i = 0;
            i < puzzlePieces.Length;
            i++
        ) {

            if (
                puzzlePieces[i] != null
            ) {

                puzzlePieces[i]
                    .gameObject
                    .SetActive(
                        false
                    );
            }
        }

        specialCount = 0;
    }
    // Returns collected normal coin total
    public int GetCoinCount() {

        return normalCount;
    }
}