using UnityEngine;

public class HatPurchases : MonoBehaviour {
    public Transform hatContainer;

    void Start() {
        DisableAllHats();
    }

    public void EquipHat(GameObject hat) {
        DisableAllHats();
        hat.SetActive(true);
    }

    void DisableAllHats() {
        foreach (Transform hat in hatContainer) {
            hat.gameObject.SetActive(false);
        }
    }
}