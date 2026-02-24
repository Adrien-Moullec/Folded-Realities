using Unity.Netcode;
using UnityEngine;

public class LobbyManager : NetworkManager {
    public static LobbyManager Instance;

    [SerializeField] private NetworkObject playerObject;
    [SerializeField] private NetworkObject playerDataObject;

    PlayerGameData pgd;
    NetworkManager nm;

    private void Awake() {
        Instance = this;

        pgd = PlayerGameData.Instance;
        nm = Singleton;
    }

    void Start() {
        if (!IsServer) return;
        else Debug.Log("");

        Initialize();

        SpawnAllClients();
        nm.OnClientConnectedCallback += HandleClientConnect;
        nm.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    void Update() {
        if (!nm.IsConnectedClient)
            Destroy(gameObject);
    }

    private void OnDisable() {
        Instance = null;

        if (IsServer) {
            nm.OnClientConnectedCallback -= HandleClientConnect;
            nm.OnClientDisconnectCallback -= HandleClientDisconnect;
        }

        if (!nm.IsConnectedClient) {
            if (pgd != null)
                Destroy(pgd.gameObject);
            if (nm != null)
                Destroy(nm.gameObject);

            //SceneManager.LoadScene(0);
        }
    }

    void Initialize() {
        if (pgd == null) {
            NetworkObject pd = Instantiate(playerDataObject);
            pd.Spawn();
        }
    }

    void HandleClientConnect(ulong cliendId) {
        NetworkObject player = Instantiate(playerObject);
        player.SpawnAsPlayerObject(cliendId);
    }

    void HandleClientDisconnect(ulong cliendId) {
        //nm.ConnectedClients[cliendId].PlayerObject.Despawn(true);
    }

    void SpawnAllClients() {
        foreach (ulong clientId in nm.ConnectedClientsIds)
            if (nm.ConnectedClients[clientId].PlayerObject != null) {
                nm.ConnectedClients[clientId].PlayerObject.Despawn(true);
            }
        //nm.SceneManager.LoadScene(loadScene.ToString(), LoadSceneMode.Single);
    }
}
