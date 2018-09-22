using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : SingletonBehaviour<LobbyManager> 
{
    #region Member Variables

        [SerializeField]
	    private SO_LobbyDetails _LobbyDetails;

	    private bool _IsReady = false;

    #endregion

    #region  Life Cycle

    	override protected void SingletonOnEnable() 
		{
			PhotonNetwork.NetworkingClient.EventReceived += OnNetworkEvent;
		}

		override protected void SingletonOnDisable()
		{
			PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEvent;
		}

		override protected void SingletonStart()
		{
			// Initializing the Lobby Details
			_LobbyDetails.Initialize(PhotonNetworkManager.Instance.MaximumNumberOfPlayersInARoom);
		}

		override protected void SingletonUpdate()
		{
			// Updating the player count of the room
			if (PhotonNetwork.InRoom)
			{
				_LobbyDetails.NumberOfPlayersInRoom = PhotonNetwork.CurrentRoom.PlayerCount; 
			}
		}

	#endregion

	#region Member Functions
	
		public void ReadyUp()
		{
			if (!_IsReady)
			{
				// Configuring the event
				RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.AddToRoomCache };
				SendOptions sendOptions = new SendOptions { Reliability = true };
				PhotonNetwork.RaiseEvent((byte)LobbyNetworkedEvents.PLAYER_READIED_UP, null, eventOptions, sendOptions);
				_IsReady = true;
			}
		}

		private bool CanStartGame()
		{
			return PhotonNetwork.CurrentRoom.MaxPlayers == _LobbyDetails.NumberOfPlayersReady;
		}

		private void StartGame()
		{
			if (PhotonNetwork.IsMasterClient)
			{
				// Using Photon to load the level to make sure that all the clients load the same level
				Debug.Log("Starting Game...");
				PhotonNetwork.LoadLevel("Level_" + _LobbyDetails.LevelToLoad);
				return;
			}

			Debug.Log("Waiting for Master Client to load the level...");
		}

	#endregion

	#region Network Callbacks

		private void OnNetworkEvent(EventData photonNetworkEvent)
		{
			byte eventCode = photonNetworkEvent.Code;  
			
			if (eventCode == (byte)LobbyNetworkedEvents.PLAYER_READIED_UP)
			{
				OnPlayerReadiedUp();
			}
		}

		private void OnPlayerReadiedUp()
		{
			_LobbyDetails.NumberOfPlayersReady += 1;

			if (CanStartGame())
			{
				StartGame();
			}
		}

	#endregion

	#region Network Events

		private enum LobbyNetworkedEvents : byte
		{
			PLAYER_READIED_UP = 0
		}

	#endregion
}
