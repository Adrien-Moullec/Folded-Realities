using UnityEngine;

public class CurrencyManager : MonoBehaviour {
    public static CurrencyManager Instance;

    public int coins = 0;

    void Awake() {

        Instance = this;

        coins = GameplaySystem.GetInt(PrefInt.Coins, 0);
    }

    public void AddCoins(int amount) {
        coins += amount;
        GameplaySystem.SetInt(PrefInt.Coins, coins);
    }

    public bool SpendCoins(int amount) {
        if (coins >= amount) {
            coins -= amount;
            GameplaySystem.SetInt(PrefInt.Coins, coins);
            return true;
        }

        return false;
    }
}