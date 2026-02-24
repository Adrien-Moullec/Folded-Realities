using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RelayManager : NetworkBehaviour, IRelay {

    public string joinCode;

    private async void Start() {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    async void HostRelay() {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(7);
        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        var relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartHost();

        OnHostReplay();
    }

    async void JoinRelay(string joinCode) {
        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        var relayServerData = AllocationUtils.ToRelayServerData(joinAllocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartClient();
    }

    async void OnHostReplay() {
        //NetworkManager.Singleton.SceneManager.LoadScene("", LoadSceneMode.Single);
    }
}