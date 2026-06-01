using UnityEngine;

public class PlayerHatLoader : MonoBehaviour {

    public GameObject crownHat;
    public GameObject boxHat;

    void Start() {

        crownHat.SetActive(false);
        boxHat.SetActive(false);

        int equippedHat =
            GameplaySystem.GetInt(
                PrefInt.EquippedHat,
                0
            );

        switch (equippedHat) {

            case 1:
                crownHat.SetActive(true);
                break;

            case 2:
                boxHat.SetActive(true);
                break;
        }
    }
}