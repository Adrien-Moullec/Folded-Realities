using UnityEngine;

using TMPro;

public class VMInteract : MonoBehaviour {

    [Header("UI")]
    public GameObject shopUI;
    [Header("Player")]
    public GameObject playerVisuals;
    public MonoBehaviour playerController;
    [Header("Currency")]
    public int playerCoins = 0;
    [Header("Hat Costs")]
    public int crownCost = 30;
    public int boxHatCost = 15;

    [Header("Price Text")]
    public TMP_Text crownPriceText;
    public TMP_Text boxHatPriceText;
    [Header("Owned Hats")]
    public bool ownsCrown = false;
    public bool ownsBoxHat = false;
    [Header("Hats")]
    public Transform hatContainer;
    public GameObject crownHat;
    public GameObject boxHat;
    bool shopOpen = false;

    void Start() {

        // LOAD COINS
        playerCoins = GameplaySystem.GetInt(PrefInt.Coins, 0);
        // LOAD PURCHASES
        ownsCrown = GameplaySystem.GetInt(PrefInt.OwnsCrown, 0) == 1;
        ownsBoxHat = GameplaySystem.GetInt(PrefInt.OwnsBoxHat, 0) == 1;
        // hide shop
        if (shopUI != null) shopUI.SetActive(false);

        // disable all hats
        if (hatContainer != null) {
            foreach (Transform h in hatContainer) {
                h.gameObject.SetActive(false);
            }
        }

        // already owned = free
        if (ownsCrown) crownCost = 0;
        if (ownsBoxHat) boxHatCost = 0;

        // UPDATE UI
        UpdatePriceUI();
    }

    void OnTriggerStay(Collider other) {

        if (
            !other.CompareTag("Player")
        ) return;

        if (!shopOpen) {

            Debug.Log(
                "Opening Shop"
            );

            OpenShop();
        }
    }

    void OpenShop() {

        shopOpen = true;

        // SHOW SHOP UI
        if (shopUI != null)
            shopUI.SetActive(true);

        // FREEZE PLAYER
        if (playerController != null)
            playerController.enabled = false;

        // HIDE PLAYER
        if (playerVisuals != null)
            playerVisuals.SetActive(false);

        // UNLOCK CURSOR
        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        Debug.Log(
            "Shop Opened"
        );
    }

    public void CloseShop() {

        shopOpen = false;

        // HIDE SHOP UI
        if (shopUI != null)
            shopUI.SetActive(false);

        // ENABLE PLAYER
        if (playerController != null)
            playerController.enabled = true;

        // SHOW PLAYER
        if (playerVisuals != null)
            playerVisuals.SetActive(true);

        // LOCK CURSOR
        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        Debug.Log(
            "Shop Closed"
        );
    }

    // BUY / EQUIP CROWN
    public void BuyCrown() {

        // already owned
        if (ownsCrown) {

            Debug.Log(
                "Crown Already Owned"
            );

            EquipHat(crownHat);

            return;
        }

        // check coins
        if (
            playerCoins <
            crownCost
        ) {

            Debug.Log(
                "Not Enough Coins"
            );

            return;
        }

        // buy
        playerCoins -= crownCost;

        ownsCrown = true;

        crownCost = 0;

        SaveData();

        UpdatePriceUI();

        EquipHat(crownHat);

        Debug.Log(
            "Bought Crown Hat"
        );
    }

    // BUY / EQUIP BOX HAT
    public void BuyBoxHat() {

        // already owned
        if (ownsBoxHat) {

            Debug.Log(
                "Box Hat Already Owned"
            );

            EquipHat(boxHat);

            return;
        }

        // check coins
        if (
            playerCoins <
            boxHatCost
        ) {

            Debug.Log(
                "Not Enough Coins"
            );

            return;
        }

        // buy
        playerCoins -= boxHatCost;

        ownsBoxHat = true;

        boxHatCost = 0;

        SaveData();

        UpdatePriceUI();

        EquipHat(boxHat);

        Debug.Log(
            "Bought Box Hat"
        );
    }

    // EQUIP ONLY ONE HAT
    void EquipHat(GameObject hat) {

        if (hat == null) {

            Debug.LogError(
                "HAT IS NULL"
            );

            return;
        }

        // disable ALL hats
        foreach (
            Transform h
            in hatContainer
        ) {

            h.gameObject.SetActive(false);
        }

        // enable selected hat
        hat.SetActive(true);

        Debug.Log(
            "Equipped Hat: "
            + hat.name
        );
    }

    // UPDATE UI TEXT
    void UpdatePriceUI() {

        if (crownPriceText != null)
            crownPriceText.text =
                crownCost.ToString();

        if (boxHatPriceText != null)
            boxHatPriceText.text =
                boxHatCost.ToString();
    }

    // SAVE DATA
    void SaveData() {

        GameplaySystem.SetInt(PrefInt.Coins, playerCoins);
        GameplaySystem.SetInt(PrefInt.OwnsCrown, ownsCrown ? 1 : 0);
        GameplaySystem.SetInt(PrefInt.OwnsBoxHat, ownsBoxHat ? 1 : 0);
        GameplaySystem.SaveSettings();
    }

    // DEBUG BUTTON
    [ContextMenu("Add 10 Coins")]
    void AddCoinsDebug() {
        playerCoins += 10;
        SaveData();
    }
}