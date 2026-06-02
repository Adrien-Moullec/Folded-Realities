using UnityEngine;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour {

    public static KeyManager Instance;

    [Header("UI Images")]
    public Image key1UI;
    public Image key2UI;
    public Image key3UI;

    [Header("Object To Unlock")]
    public GameObject cage;

    private int keysCollected = 0;

    private void Awake() {
        Instance = this;
    }

    private void Start() {

        key1UI.enabled = false;
        key2UI.enabled = false;
        key3UI.enabled = false;
    }

    public void CollectKey() {


        keysCollected++;

        Debug.Log("Keys Collected: " + keysCollected);

        if (keysCollected >= 1)
            key1UI.enabled = true;

        if (keysCollected >= 2)
            key2UI.enabled = true;

        if (keysCollected >= 3) {

            key3UI.enabled = true;

            Debug.Log("Removing Cage");

            if (cage != null) {
                cage.SetActive(false);
            }
        }
    }

}