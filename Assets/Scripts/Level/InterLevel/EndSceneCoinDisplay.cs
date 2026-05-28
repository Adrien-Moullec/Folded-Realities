using UnityEngine;

using TMPro;

public class EndSceneCoinsDisplay : MonoBehaviour {

    public TextMeshProUGUI coinsText;
    // Updates UI with saved coin total
    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        coinsText.text = "Coins Collected: " + GameplaySystem.GetInt(PrefInt.Coins, 0);
    }
}