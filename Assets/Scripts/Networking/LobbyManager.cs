using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using LLAPI;
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

        protected override void SingletonOnEnable() 
		{
			PhotonNetwork.NetworkingClient.EventReceived += OnNetworkEvent;
		}

        protected override void SingletonOnDisable()
		{
			PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEvent;
		}

		protected override void SingletonUpdate()
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
			if (!_IsReady && PhotonNetwork.InRoom)
			{
			    Debug.Log("Readying up...");

				// Configuring the event
				RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.AddToRoomCache };
				SendOptions sendOptions = new SendOptions { Reliability = true };
				PhotonNetwork.RaiseEvent((byte)LobbyNetworkedEvents.PLAYER_READIED_UP, null, eventOptions, sendOptions);
				_IsReady = true;

				 // Initializing the Lobby Details
				if (PhotonNetwork.IsMasterClient)
				{
					int[] indices = _LobbyDetails.Initialize(PhotonNetworkManager.Instance.MaximumNumberOfPlayersInARoom);
					Byterizer byterizer = new Byterizer();
					byterizer.Push(indices[0]);
					byterizer.Push(indices[1]);

					PhotonNetwork.RaiseEvent((byte)LobbyNetworkedEvents.CHOOSE_JUDGE_AND_DISH, null, eventOptions, sendOptions);
				}
			}
		}

		private bool CanStartGame()
		{
			return PhotonNetwork.CurrentRoom.MaxPlayers == _LobbyDetails.NumberOfPlayersReady;
		}

		private void StartGame()
		{
            // Loading the level only on the master client
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

		    else if (eventCode == (byte)LobbyNetworkedEvents.CHOOSE_JUDGE_AND_DISH)
		    {
                Byterizer byterizer = new Byterizer();
                byterizer.LoadDeep((byte[])photonNetworkEvent.CustomData);
                
		        int judgeIndex = byterizer.PopInt32();
                int dishIndex = byterizer.PopInt32();

		        _LobbyDetails.Initialize(PhotonNetworkManager.Instance.MaximumNumberOfPlayersInARoom, judgeIndex, dishIndex);
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
			PLAYER_READIED_UP = 0,
            CHOOSE_JUDGE_AND_DISH
		}

	#endregion
}
