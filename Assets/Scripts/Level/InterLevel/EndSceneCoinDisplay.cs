using UnityEngine;

using TMPro;

public class EndSceneCoinsDisplay : MonoBehaviour {

    public TextMeshProUGUI coinsText;
    // Updates UI with saved coin total
    void Start() {
        coinsText.text = "Coins Collected: " + GameplaySystem.GetInt(PrefInt.Coins, 0);
    }
}