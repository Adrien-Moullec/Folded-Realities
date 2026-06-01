using UnityEngine;

public class HatPurchases : MonoBehaviour {

    public Transform hatContainer;

    public GameObject crownHat;
    public GameObject boxHat;

    void Awake() {

        ApplySavedHat();
    }

    void ApplySavedHat() {

        DisableAllHats();

        int equippedHat =
            GameplaySystem.GetInt(
                PrefInt.EquippedHat,
                0
            );

        switch (equippedHat) {

            case 1:

                if (crownHat != null)
                    crownHat.SetActive(true);

                break;

            case 2:

                if (boxHat != null)
                    boxHat.SetActive(true);

                break;
        }
    }

    public void EquipHat(
        GameObject hat,
        int hatID
    ) {

        DisableAllHats();

        if (hat != null)
            hat.SetActive(true);

        GameplaySystem.SetInt(
            PrefInt.EquippedHat,
            hatID
        );

        GameplaySystem.SaveSettings();
    }

    void DisableAllHats() {

        if (hatContainer == null)
            return;

        foreach (Transform hat in hatContainer) {

            hat.gameObject.SetActive(false);
        }
    }
}