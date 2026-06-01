using UnityEngine;
using TMPro;

public class VMInteract : MonoBehaviour {

    #region References

    [Header("UI")]
    public GameObject shopUI;

    [Header("Player")]
    public Renderer[] playerRenderers;
    public MonoBehaviour playerController;

    [Header("Currency")]
    public int playerCoins = 0;

    #endregion

    #region Costs

    [Header("Hat Costs")]
    public int crownCost = 30;
    public int boxHatCost = 30;

    [Header("Price Text")]
    public TMP_Text crownPriceText;
    public TMP_Text boxHatPriceText;

    #endregion

    #region Hats

    [Header("Owned Hats")]
    public bool ownsCrown;
    public bool ownsBoxHat;

    [Header("Hats")]
    public Transform hatContainer;
    public GameObject crownHat;
    public GameObject boxHat;

    GameObject equippedHat;

    #endregion

    #region Shop Logic

    bool shopOpen = false;
    bool playerWasInside = false;

    void Start() {

        playerCoins =
            GameplaySystem.GetInt(
                PrefInt.Coins,
                0
            );

        ownsCrown =
            GameplaySystem.GetInt(
                PrefInt.OwnsCrown,
                0
            ) == 1;

        ownsBoxHat =
            GameplaySystem.GetInt(
                PrefInt.OwnsBoxHat,
                0
            ) == 1;

        if (shopUI != null)
            shopUI.SetActive(false);

        DisableAllHats();

        if (ownsCrown)
            crownCost = 0;

        if (ownsBoxHat)
            boxHatCost = 0;

        LoadEquippedHat();

        UpdatePriceUI();
    }

    void Update() {

        Collider[] hits =
            Physics.OverlapBox(
                transform.position,
                transform.localScale / 2
            );

        bool playerInside = false;

        foreach (Collider c in hits) {

            if (c.CompareTag("Player")) {

                playerInside = true;
                break;
            }
        }

        if (
            playerInside &&
            !playerWasInside &&
            !shopOpen
        ) {
            OpenShop();
        }

        if (!playerInside) {

            playerWasInside = false;
        } else {

            playerWasInside = true;
        }
    }

    void OpenShop() {

        shopOpen = true;

        if (shopUI != null)
            shopUI.SetActive(true);

        if (playerController != null)
            playerController.enabled = false;

        foreach (Renderer r in playerRenderers) {

            if (r != null)
                r.enabled = false;
        }

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;
    }

    public void CloseShop() {

        shopOpen = false;

        if (shopUI != null)
            shopUI.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        foreach (Renderer r in playerRenderers) {

            if (r != null)
                r.enabled = true;
        }

        if (equippedHat != null)
            equippedHat.SetActive(true);

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    #endregion

    #region Purchases

    public void BuyCrown() {

        if (ownsCrown) {

            EquipHat(crownHat, 1);
            return;
        }

        if (playerCoins < crownCost)
            return;

        playerCoins -= crownCost;

        ownsCrown = true;

        crownCost = 0;

        SaveData();

        UpdatePriceUI();

        EquipHat(crownHat, 1);
    }

    public void BuyBoxHat() {

        if (ownsBoxHat) {

            EquipHat(boxHat, 2);
            return;
        }

        if (playerCoins < boxHatCost)
            return;

        playerCoins -= boxHatCost;

        ownsBoxHat = true;

        boxHatCost = 0;

        SaveData();

        UpdatePriceUI();

        EquipHat(boxHat, 2);
    }

    void EquipHat(
        GameObject hat,
        int hatID
    ) {

        if (hat == null)
            return;

        DisableAllHats();

        hat.SetActive(true);

        equippedHat = hat;

        GameplaySystem.SetInt(
            PrefInt.EquippedHat,
            hatID
        );

        GameplaySystem.SaveSettings();
    }

    void DisableAllHats() {

        if (hatContainer == null)
            return;

        foreach (Transform h in hatContainer) {

            h.gameObject.SetActive(false);
        }
    }

    void LoadEquippedHat() {

        int equippedID =
            GameplaySystem.GetInt(
                PrefInt.EquippedHat,
                0
            );

        switch (equippedID) {

            case 1:

                if (ownsCrown)
                    EquipHat(
                        crownHat,
                        1
                    );

                break;

            case 2:

                if (ownsBoxHat)
                    EquipHat(
                        boxHat,
                        2
                    );

                break;
        }
    }

    #endregion

    #region UI

    void UpdatePriceUI() {

        if (crownPriceText != null)
            crownPriceText.text =
                crownCost.ToString();

        if (boxHatPriceText != null)
            boxHatPriceText.text =
                boxHatCost.ToString();
    }

    #endregion

    #region Save

    void SaveData() {

        GameplaySystem.SetInt(
            PrefInt.Coins,
            playerCoins
        );

        GameplaySystem.SetInt(
            PrefInt.OwnsCrown,
            ownsCrown ? 1 : 0
        );

        GameplaySystem.SetInt(
            PrefInt.OwnsBoxHat,
            ownsBoxHat ? 1 : 0
        );

        GameplaySystem.SaveSettings();
    }

    #endregion

    [ContextMenu("Add 10 Coins")]
    void AddCoinsDebug() {

        playerCoins += 10;

        SaveData();
    }
}