using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class CollectiblesManager : MonoBehaviour {

    public static CollectiblesManager Instance;

    public int levelID = 0;

    public int normalCount = 0;

    public TMP_Text normalCountText;

    public GameObject specialCanvas;

    public Image[] puzzlePieces;

    private int specialCount = 0;

    public AudioClip pickupSound;

    void Awake() {

        Instance = this;

        Debug.Log(
            "COLLECTIBLES MANAGER AWAKE"
        );
    }

    void Start() {

        Debug.Log(
            "COLLECTIBLES MANAGER START"
        );

        normalCount = 0;

        UpdateNormalUI();

        ResetPuzzleUI();

        if (specialCanvas != null) {

            specialCanvas.SetActive(true);

            Debug.Log(
                "SPECIAL CANVAS ENABLED"
            );
        } else {

            Debug.LogError(
                "SPECIAL CANVAS IS NULL"
            );
        }

        Debug.Log(
            "PUZZLE PIECE COUNT: "
            + puzzlePieces.Length
        );

        for (
            int i = 0;
            i < puzzlePieces.Length;
            i++
        ) {

            if (
                puzzlePieces[i] == null
            ) {

                Debug.LogError(
                    "PUZZLE PIECE NULL AT INDEX: "
                    + i
                );
            } else {

                Debug.Log(
                    "PUZZLE PIECE ASSIGNED: "
                    + puzzlePieces[i].name
                );
            }
        }
    }

    public void CollectNormal(
        GameObject obj
    ) {

        Debug.Log(
            "NORMAL COLLECTIBLE PICKED UP"
        );

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
    }

    public void CollectSpecial(
        GameObject obj
    ) {

        Debug.Log(
            "SPECIAL COLLECTIBLE PICKED UP"
        );

        Debug.Log(
            "CURRENT SPECIAL COUNT: "
            + specialCount
        );

        if (
            specialCount
            >=
            puzzlePieces.Length
        ) {

            Debug.LogError(
                "SPECIAL COUNT EXCEEDS ARRAY"
            );

            return;
        }

        if (
            puzzlePieces[specialCount]
            == null
        ) {

            Debug.LogError(
                "PUZZLE PIECE IS NULL AT INDEX: "
                + specialCount
            );

            return;
        }

        Debug.Log(
            "ENABLING IMAGE: "
            + puzzlePieces[specialCount].name
        );

        puzzlePieces[specialCount]
            .gameObject
            .SetActive(true);

        Debug.Log(
            "IMAGE ENABLED SUCCESSFULLY"
        );

        specialCount++;

        Debug.Log(
            "NEW SPECIAL COUNT: "
            + specialCount
        );

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

            Debug.Log(
                "ALL SPECIAL PIECES FOUND"
            );

            OnPuzzleCompleted();
        }
    }

    void OnPuzzleCompleted() {

        Debug.Log(
            "PUZZLE COMPLETE"
        );

        PlayerPrefs.SetInt(
            "PuzzleComplete_Level_"
            + levelID,
            1
        );

        PlayerPrefs.Save();
    }

    void UpdateNormalUI() {

        if (
            normalCountText != null
        ) {

            normalCountText.text =
                normalCount.ToString();

            Debug.Log(
                "UPDATED NORMAL UI: "
                + normalCount
            );
        } else {

            Debug.LogError(
                "NORMAL COUNT TEXT NULL"
            );
        }
    }

    void ResetPuzzleUI() {

        Debug.Log(
            "RESETTING PUZZLE UI"
        );

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
                    .SetActive(false);

                Debug.Log(
                    "RESET IMAGE: "
                    + puzzlePieces[i].name
                );
            }
        }

        specialCount = 0;

        Debug.Log(
            "SPECIAL COUNT RESET"
        );
    }

    public int GetCoinCount() {

        return normalCount;
    }
}