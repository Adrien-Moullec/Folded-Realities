using UnityEngine;

using TMPro;

public class EndSceneCoinsDisplay : MonoBehaviour {

    public TextMeshProUGUI coinsText;

    void Start() {
        int coins = GameplaySystem.GetInt(PrefInt.Coins, 0);
        coinsText.text = "Coins Collected: " + coins;
    }
}