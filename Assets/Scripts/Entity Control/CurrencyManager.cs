using UnityEngine;

public class CurrencyManager : MonoBehaviour {

    public static CurrencyManager Instance;

    // Tracks total player coins
    public int coins = 0;

    void Awake() {

        Instance = this;

        // Loads saved coin count
        coins = GameplaySystem.GetInt(PrefInt.Coins, 0);
    }

    public void AddCoins(int amount) {

        // Adds coins and updates save data
        coins += amount;

        GameplaySystem.SetInt(PrefInt.Coins, coins);
    }

    public bool SpendCoins(int amount) {

        // Checks player has enough coins
        if (coins >= amount) {

            coins -= amount;

            GameplaySystem.SetInt(PrefInt.Coins, coins);

            return true;
        }

        return false;
    }
}