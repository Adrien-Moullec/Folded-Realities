using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkCanvas : NetworkBehaviour {
    [SerializeField] GameObject Visuals;
    [SerializeField] List<TextMeshProUGUI> playerCards;

    PlayerGameData pgd;
    NetworkManager nm;

    public override void OnNetworkSpawn() {
        Initialize();
    }

    void OnDisable() {
        pgd.Players.OnListChanged -= input => OnUpdatePlayerInfo();
    }

    private void Initialize() {
        if (!IsOwner) return;

        nm = NetworkManager.Singleton;
        pgd = PlayerGameData.Instance;

        if (IsHost) {
            //Set host logic
        }

        Visuals.SetActive(true);

        pgd.Players.OnListChanged += input => OnUpdatePlayerInfo();
    }

    void OnUpdatePlayerInfo() {
        if (!IsOwner) return;

        foreach (TextMeshProUGUI player in playerCards)
            player.text = "...";

        for (int i = 0; i < Mathf.Min(playerCards.Count, pgd.Players.Count); i++) {
            playerCards[i].text = pgd.Players[i].Name.ToString();
        }
    }
}
