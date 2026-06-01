using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour {

    public static CurrencyManager Instance;

    // Tracks total player coins
    public int coins = 0;

    [Header("UI")]
    public TMP_Text coinText;

    void Awake() {

        Instance = this;

        // Loads saved coin count
        coins = GameplaySystem.GetInt(
            PrefInt.Coins,
            0
        );

        UpdateCoinUI();
    }

    public void AddCoins(int amount) {

        // Adds coins and updates save data
        coins += amount;

        GameplaySystem.SetInt(
            PrefInt.Coins,
            coins
        );

        GameplaySystem.SaveSettings();

        UpdateCoinUI();
    }

    public bool SpendCoins(int amount) {

        // Checks player has enough coins
        if (coins >= amount) {

            coins -= amount;

            GameplaySystem.SetInt(
                PrefInt.Coins,
                coins
            );

            GameplaySystem.SaveSettings();

            UpdateCoinUI();

            return true;
        }

        return false;
    }

    public void UpdateCoinUI() {

        if (coinText != null) {

            coinText.text =
                coins.ToString();
        }
    }
}