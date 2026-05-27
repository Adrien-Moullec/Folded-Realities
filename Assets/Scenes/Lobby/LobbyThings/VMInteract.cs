using UnityEngine;
using TMPro;

public class VMInteract : MonoBehaviour {

    #region References

    [Header("UI")]
    public GameObject shopUI;

    [Header("Player")]
    public GameObject playerVisuals;
    public MonoBehaviour playerController;

    [Header("Currency")]
    public int playerCoins = 0;

    #endregion

    #region Costs

    [Header("Hat Costs")]
    public int crownCost = 30;
    public int boxHatCost = 15;

    [Header("Price Text")]
    public TMP_Text crownPriceText;
    public TMP_Text boxHatPriceText;

    #endregion

    #region Hats

    [Header("Owned Hats")]
    public bool ownsCrown = false;
    public bool ownsBoxHat = false;

    [Header("Hats")]
    public Transform hatContainer;
    public GameObject crownHat;
    public GameObject boxHat;

    #endregion

    #region Shop Logic
    bool shopOpen = false;

    void Start() {

        // Load saved currency and purchases
        playerCoins = GameplaySystem.GetInt(PrefInt.Coins, 0);

        ownsCrown = GameplaySystem.GetInt(PrefInt.OwnsCrown, 0) == 1;

        ownsBoxHat = GameplaySystem.GetInt(PrefInt.OwnsBoxHat, 0) == 1;

        if (shopUI != null)
            shopUI.SetActive(false);

        // Disable all hats on startup
        if (hatContainer != null) {

            foreach (Transform h in hatContainer)
                h.gameObject.SetActive(false);
        }

        // Owned hats become free
        if (ownsCrown)
            crownCost = 0;

        if (ownsBoxHat)
            boxHatCost = 0;

        UpdatePriceUI();
    }

    void OnTriggerStay(Collider other) {

        if (!other.CompareTag("Player"))
            return;

        if (!shopOpen)
            OpenShop();
    }

    void OpenShop() {

        shopOpen = true;

        // Opens shop and disables player control
        if (shopUI != null)
            shopUI.SetActive(true);

        if (playerController != null)
            playerController.enabled = false;

        if (playerVisuals != null)
            playerVisuals.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseShop() {

        shopOpen = false;

        // Restores gameplay controls
        if (shopUI != null)
            shopUI.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        if (playerVisuals != null)
            playerVisuals.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BuyCrown() {

        // Equips already owned hat
        if (ownsCrown) {

            EquipHat(crownHat);

            return;
        }

        if (playerCoins < crownCost)
            return;

        playerCoins -= crownCost;

        ownsCrown = true;

        crownCost = 0;

        SaveData();

        UpdatePriceUI();

        EquipHat(crownHat);
    }

    public void BuyBoxHat() {

        // Equips already owned hat
        if (ownsBoxHat) {

            EquipHat(boxHat);

            return;
        }

        if (playerCoins < boxHatCost)
            return;

        playerCoins -= boxHatCost;

        ownsBoxHat = true;

        boxHatCost = 0;

        SaveData();

        UpdatePriceUI();

        EquipHat(boxHat);
    }

    void EquipHat(GameObject hat) {

        if (hat == null)
            return;

        // Ensures only one hat is active
        foreach (Transform h in hatContainer)
            h.gameObject.SetActive(false);

        hat.SetActive(true);
    }

    void UpdatePriceUI() {

        // Updates shop price text
        if (crownPriceText != null)
            crownPriceText.text = crownCost.ToString();

        if (boxHatPriceText != null)
            boxHatPriceText.text = boxHatCost.ToString();
    }

    void SaveData() {

        // Saves purchases and coin count
        GameplaySystem.SetInt(PrefInt.Coins, playerCoins);

        GameplaySystem.SetInt(PrefInt.OwnsCrown, ownsCrown ? 1 : 0);

        GameplaySystem.SetInt(PrefInt.OwnsBoxHat, ownsBoxHat ? 1 : 0);

        GameplaySystem.SaveSettings();
    }
    #endregion

    [ContextMenu("Add 10 Coins")]
    void AddCoinsDebug() {

        // Debug shortcut for testing purchases
        playerCoins += 10;

        SaveData();
    }
}