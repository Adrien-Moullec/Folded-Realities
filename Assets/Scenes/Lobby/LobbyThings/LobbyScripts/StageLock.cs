using UnityEngine;

public class StageLock : MonoBehaviour {
    public string requiredKey; 
    public GameObject lockVisual; 
    public GameObject lockedUI; 

    private bool isUnlocked = false;

    void Start() {
       
        if (lockVisual != null)
            lockVisual.SetActive(true);

        isUnlocked = PlayerPrefs.GetInt(requiredKey, 0) == 1;
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;

        isUnlocked = PlayerPrefs.GetInt(requiredKey, 0) == 1;

        if (isUnlocked) {
            UnlockVisual();
            Debug.Log("Unlocked - allow access");

            
        } else {
            Debug.Log("Still locked");

            if (lockedUI != null)
                lockedUI.SetActive(true);
        }
    }

    void UnlockVisual() {
        if (lockVisual != null && lockVisual.activeSelf) {
            lockVisual.SetActive(false);
        }
    }
}