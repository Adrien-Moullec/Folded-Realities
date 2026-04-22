using UnityEngine;

public class CurrencyManager : MonoBehaviour {
    public static CurrencyManager Instance;

    public int coins = 0;

    void Awake() {
        if (Instance != null && Instance != this)
            Destroy(this);
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            coins = PlayerPrefs.GetInt("Coins", 0);
        }
    }

    public void AddCoins(int amount) {
        coins += amount;
        PlayerPrefs.SetInt("Coins", coins);
    }

    public bool SpendCoins(int amount) {
        if (coins >= amount) {
            coins -= amount;
            PlayerPrefs.SetInt("Coins", coins);
            return true;
        }

        return false;
    }
}