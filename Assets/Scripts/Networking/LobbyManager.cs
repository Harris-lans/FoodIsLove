using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using LLAPI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : SingletonBehaviour<LobbyManager> 
{
    #region Member Variables

        [SerializeField]
	    private SO_LobbyDetails _LobbyDetails;

		[SerializeField]
		private SO_HeroList _HeroList;

		[SerializeField]
		private SO_Tag _GameStartScreen;

		public UnityEvent OnPlayerStatusChange;

	    private bool _IsReady = false;
		private PhotonNetworkManager _PhotonNetworkManager;
		private UIManager _UIManager;

    #endregion

    #region  Life Cycle

        protected override void SingletonOnEnable() 
		{
			PhotonNetwork.NetworkingClient.EventReceived += OnNetworkEvent;
			_UIManager = UIManager.Instance;
		}

		protected override void SingletonStart()
		{
			_PhotonNetworkManager = PhotonNetworkManager.Instance;

			// Subscribing to events
			_PhotonNetworkManager.OnLocalPlayerJoinedRoomEvent.AddListener(OnLocalPlayerEnteredRoom);
			_PhotonNetworkManager.OnLocalPlayerLeftRoomEvent.AddListener(OnLocalPlayerLeftRoom);
		}

		protected override void SingletonUpdate()
		{
			// Updating the player count of the room
			if (PhotonNetwork.InRoom)
			{
				_LobbyDetails.NumberOfPlayersInRoom = PhotonNetwork.CurrentRoom.PlayerCount; 
			}
		}

		protected override void SingletonOnDisable()
		{
			PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEvent;
		}

	#endregion

	#region Member Functions

		private void OnLocalPlayerLeftRoom()
		{
			// Resetting the Lobby Variables
			_LobbyDetails.InitializeLobbyConnectionDetails(_PhotonNetworkManager.MaximumNumberOfPlayersInARoom);
		}

		private void OnLocalPlayerEnteredRoom()
		{
			// Initializing the lobby
			InitializeLobby();
		}
	
		public void InitializeLobby()
		{
			Debug.Log("Initializing the lobby");
			// Initializing the Lobby Details
			// Only the master client can choose the Judge to
			_IsReady = false;
			_LobbyDetails.InitializeLobbyConnectionDetails(PhotonNetworkManager.Instance.MaximumNumberOfPlayersInARoom);
			
			if (PhotonNetwork.IsMasterClient)
			{
				int[] indices = _LobbyDetails.Initialize();
				RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.AddToRoomCache };
				SendOptions sendOptions = new SendOptions { Reliability = true };
				Byterizer byterizer = new Byterizer();
				byterizer.Push(indices[0]);
				byterizer.Push(indices[1]);
				byte[] data = byterizer.GetBuffer();

				PhotonNetwork.RaiseEvent((byte)LobbyNetworkedEvents.CHOOSE_JUDGE_AND_DISH, data, eventOptions, sendOptions);
			}
		}

		public void ReadyUp(int indexOfHeroData)
		{
			if (!_IsReady && PhotonNetwork.InRoom)
			{
			    Debug.Log("Readying up...");

				// Configuring the event
				RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };
				SendOptions sendOptions = new SendOptions { Reliability = true };
				Byterizer byterizer = new Byterizer();
				byterizer.Push(indexOfHeroData);
				byte[] data = byterizer.GetBuffer();
				PhotonNetwork.RaiseEvent((byte)LobbyNetworkedEvents.PLAYER_READIED_UP, data, eventOptions, sendOptions);
				_IsReady = true;

				OnPlayerReadiedUp(true, indexOfHeroData);
			}
		}

		public void NotReady()
		{
			if (_IsReady && PhotonNetwork.InRoom)
			{
				// Configuring the event
				RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };
				SendOptions sendOptions = new SendOptions { Reliability = true };
				PhotonNetwork.RaiseEvent((byte)LobbyNetworkedEvents.PLAYER_NOT_READY, null, eventOptions, sendOptions);
				_IsReady = false;

				OnPlayerNotReady(false);
			}
		}

		private bool CanStartGame()
		{
			return PhotonNetwork.CurrentRoom.MaxPlayers == _LobbyDetails.NumberOfPlayersReady;
		}

		private void StartGame()
		{
			_UIManager.SetScreen(_GameStartScreen);

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
				Byterizer byterizer = new Byterizer();
                byterizer.LoadDeep((byte[])photonNetworkEvent.CustomData);
				int indexOfChosenHero = byterizer.PopInt32();
				OnPlayerReadiedUp(false, indexOfChosenHero);
			}

			else if (eventCode == (byte)LobbyNetworkedEvents.PLAYER_NOT_READY)
			{
				OnPlayerNotReady(false);
			}

		    else if (eventCode == (byte)LobbyNetworkedEvents.CHOOSE_JUDGE_AND_DISH)
		    {
				// The master client is the first one to be initialized so it need not initialize again
				if (PhotonNetwork.IsMasterClient)
				{
					return;
				}

                Byterizer byterizer = new Byterizer();
                byterizer.LoadDeep((byte[])photonNetworkEvent.CustomData);
                
		        int judgeIndex = byterizer.PopInt32();
                int dishIndex = byterizer.PopInt32();

				_LobbyDetails.Reset();
		        _LobbyDetails.Initialize(judgeIndex, dishIndex);
		    }
    	}

		private void OnPlayerReadiedUp(bool isLocalPlayer, int indexOfChosenHero)
		{
			++_LobbyDetails.NumberOfPlayersReady;

			if (isLocalPlayer)
			{
				_LobbyDetails.ChosenHero = _HeroList.Heroes[indexOfChosenHero];
				Debug.Log("Local player Readied up");
			}
			else
			{
				_LobbyDetails.OpponentHero = _HeroList.Heroes[indexOfChosenHero];
				Debug.Log("Remote player Readied up");
			}

			OnPlayerStatusChange.Invoke();

			if (CanStartGame())
			{
				StartGame();
			}
		}

		private void OnPlayerNotReady(bool isLocalPlayer)
		{
			--_LobbyDetails.NumberOfPlayersReady;

			if (isLocalPlayer)
			{
				_LobbyDetails.ChosenHero = null;
				Debug.Log("Local player unreadied");
			}
			else
			{
				_LobbyDetails.OpponentHero = null;
				Debug.Log("Remote player unreadied");
			}

			OnPlayerStatusChange.Invoke();
		}

	#endregion

	#region Network Events

		private enum LobbyNetworkedEvents : byte
		{
			PLAYER_READIED_UP = 0,
            CHOOSE_JUDGE_AND_DISH,
			PLAYER_NOT_READY
		}

	#endregion
}
