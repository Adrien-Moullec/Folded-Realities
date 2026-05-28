using UnityEngine;

public class LobbyStateManager : MonoBehaviour {

    [Header("Doors")]
    public GameObject bedDoor;
    public GameObject chairDoor;
    public GameObject deskDoor;

    [Header("Locks")]
    public GameObject bedLock;
    public GameObject chairLock;
    public GameObject deskLock;

    void Start() {

        // Checks player progression state
        int progress = GameplaySystem.GetInt(PrefInt.Progress, 0);

        if (progress == 1)
            SetupFirstLobby();

        else if (progress == 2)
            SetupSecondLobby();

        else if (progress >= 3)
            SetupFinalLobby();
    }

    void SetupFirstLobby() {

        // First lobby state
        bedDoor.SetActive(true);

        chairDoor.SetActive(false);

        deskDoor.SetActive(false);

        if (bedLock != null)
            bedLock.SetActive(false);

        if (chairLock != null)
            chairLock.SetActive(true);

        if (deskLock != null)
            deskLock.SetActive(true);
    }

    void SetupSecondLobby() {

        // Second lobby progression state
        bedDoor.SetActive(true);

        chairDoor.SetActive(true);

        deskDoor.SetActive(false);

        if (bedLock != null)
            bedLock.SetActive(false);

        if (chairLock != null)
            chairLock.SetActive(false);

        if (deskLock != null)
            deskLock.SetActive(true);
    }

    void SetupFinalLobby() {

        // Final lobby unlock state
        bedDoor.SetActive(true);

        chairDoor.SetActive(true);

        deskDoor.SetActive(true);

        if (bedLock != null)
            bedLock.SetActive(false);

        if (chairLock != null)
            chairLock.SetActive(false);

        if (deskLock != null)
            deskLock.SetActive(false);
    }
}