using UnityEngine;
using TMPro;

public class EndSceneCoinsDisplay : MonoBehaviour {

    public TextMeshProUGUI coinsText;

    void Start() {
        int coins = PlayerPrefs.GetInt("FinalCoins", 0);
        coinsText.text = "Coins Collected: " + coins;
    }
}