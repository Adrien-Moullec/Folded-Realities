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

        int progress = GameplaySystem.instance.GetInt(PrefInt.Progress, 0);

        Debug.Log(
            "LOBBY PROGRESS: "
            + progress
        );

        if (progress == 1) {

            SetupFirstLobby();
        } else if (progress == 2) {

            SetupSecondLobby();
        } else if (progress >= 3) {

            SetupFinalLobby();
        }
    }

    void SetupFirstLobby() {

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