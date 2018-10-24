using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WaitingMenu : UIScreen
{

    [Space, Header("Screens to switch to")]
    [SerializeField]
    private SO_Tag _FoodWorldScreenTag;
    [SerializeField]
    private SO_Tag _PreparationScreenTag;

    private PhotonNetworkManager _PhotonNetworkManager;
    private LobbyManager _LobbyManager;

    private void Start()
    {
        _PhotonNetworkManager = PhotonNetworkManager.Instance;
        _LobbyManager = LobbyManager.Instance;
        _PhotonNetworkManager.OnLocalPlayerJoinedRoomEvent.AddListener(OnJoinedRoom);

        // Checking if we are already in a room
        if(_PhotonNetworkManager.InRoom)
        {
            // _LobbyManager.ReadyUp();
            _UIManager.SetScreen(_PreparationScreenTag);
        }
    }

    public void OnClickBackFoodWorld()
    {
        _PhotonNetworkManager.LeaveGame();
        _UIManager.SetScreen(_FoodWorldScreenTag);
    }

    private void OnJoinedRoom()
    {
        // _LobbyManager.ReadyUp();
        _UIManager.SetScreen(_PreparationScreenTag);
    }
}
