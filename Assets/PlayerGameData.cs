using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerGameData : NetworkBehaviour {
    public static PlayerGameData Instance;
    public NetworkList<NetworkPlayerData> Players;
    NetworkManager nm;

    private void Awake() {
        if (Instance == null) Instance = this;
        if (Players == null) Players = new NetworkList<NetworkPlayerData>();

        nm = NetworkManager.Singleton;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {

        if (!IsServer) return;

        nm.OnClientConnectedCallback += AddPlayerToListServerRpc;
        nm.OnClientDisconnectCallback += RemovePlayerFromListServerRpc;

        foreach (ulong clientId in NetworkManager.Singleton?.ConnectedClientsIds)
            AddPlayerToListServerRpc(clientId);
    }

    void OnDisable() {
        Players.Dispose();
    }

    [ServerRpc]
    public void AddPlayerToListServerRpc(ulong clientId) {
        for (int i = 0; i < Players.Count; i++)
            if (clientId == Players[i].ClientId)
                return;

        Players.Add(new NetworkPlayerData() {
            ClientId = clientId,
            Name = new FixedString32Bytes("Player " + clientId.ToString())
        });
    }

    [ServerRpc]
    public void RemovePlayerFromListServerRpc(ulong clientId) {
        for (int i = 0; i < Players.Count; i++)
            if (clientId == Players[i].ClientId) {
                Players.RemoveAt(i);
                return;
            }
    }

    public NetworkPlayerData GetPlayerData(ulong clientId) {
        for (int i = 0; i < Players.Count; i++)
            if (clientId == Players[i].ClientId)
                return Players[i];

        return new NetworkPlayerData {
            ClientId = clientId,
            Name = "Player " + clientId
        };
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void ChangePlayerNameServerRpc(ulong clientId, FixedString32Bytes newName) {
        for (int i = 0; i < Players.Count; i++)
            if (clientId == Players[i].ClientId)
                Players[i] = new NetworkPlayerData {
                    ClientId = clientId,
                    Name = Players[i].Name
                };
    }
}
