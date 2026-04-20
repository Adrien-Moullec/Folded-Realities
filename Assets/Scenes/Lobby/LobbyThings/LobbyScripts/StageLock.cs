using UnityEngine;

public class StageLock : MonoBehaviour {
    public string requiredKey;
    public GameObject lockVisual;
    public GameObject lockedUI;

    private bool isUnlocked = false;
    private DoorTeleport doorTeleport;

    void Start() {
        doorTeleport = GetComponent<DoorTeleport>();

        isUnlocked = PlayerPrefs.GetInt(requiredKey, 0) == 1;

        if (isUnlocked) {
            Unlock();
        } else {
            Lock();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;

        isUnlocked = PlayerPrefs.GetInt(requiredKey, 0) == 1;

        if (isUnlocked) {
            Unlock();
        } else {
            Lock();

            if (lockedUI != null)
                lockedUI.SetActive(true);
        }
    }

    void Unlock() {
        if (lockVisual != null)
            lockVisual.SetActive(false);

        if (doorTeleport != null)
            doorTeleport.enabled = true;
    }

    void Lock() {
        if (lockVisual != null)
            lockVisual.SetActive(true);

        if (doorTeleport != null)
            doorTeleport.enabled = false;
    }
}