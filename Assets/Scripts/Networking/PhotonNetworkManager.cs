using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
	#region Global Variables

		public static PhotonNetworkManager Instance;

		[Header("Network Details")]
		[SerializeField]
		private string _GameVersion = "v0.1";

		[Space,Header("Room Information")]
		[SerializeField]
		private byte _MaximumNumberOfPlayersInARoom = 2;
		[SerializeField]
		private int _TimeBeforeRemovingPlayerAfterDisconnect = 2000;

		#region Photon Network Events

			[Header("Photon Network Events")]
			public UnityEvent OnConnectedToMasterEvent;
			public UnityEvent OnJoinedRoomEvent;
			public UnityEvent OnCreatedRoomEvent;
			public UnityEvent OnLeftRoomEvent;
			public UnityEvent OnDisconnectedFromPhotonEvent;
			public UnityEvent OnPlayerJoinedLobby;
		
		#endregion

	#endregion

	#region Life Cycle

		private void Awake() 
		{
			PhotonNetwork.ConnectUsingSettings();
			Debug.Log("Connecting to Photon Cloud...");

			// Singleton Implementation
			if (Instance == null)
			{
				Instance = this;
			}

			else
			{
				Destroy(this);
			}
		}

		private void Start()
		{
			// Making sure that all the clients are in the same scene
			PhotonNetwork.AutomaticallySyncScene = true;
		}

	#endregion

	#region PUN Callbacks

		override public void OnConnectedToMaster()
		{
			OnConnectedToMasterEvent.Invoke();
		}

		override public void OnJoinedRoom()
		{
			OnJoinedRoomEvent.Invoke();
		}

		override public void OnCreatedRoom()
		{
			OnCreatedRoomEvent.Invoke();
		}

		override public void OnLeftRoom()
		{
			OnLeftRoomEvent.Invoke();
		}

		override public void OnDisconnected(DisconnectCause cause)
		{
			Debug.LogFormat("Cause of Disconnection: {0}", cause);
			OnDisconnectedFromPhotonEvent.Invoke();
		}

		override public void OnPlayerEnteredRoom(Player newPlayer)
		{
			OnPlayerJoinedLobby.Invoke();
		}

	#endregion

	#region Member Functions

		public void CreateOrJoinGame(string roomName)
		{
			RoomOptions roomOptions = new RoomOptions()
			{
				MaxPlayers = _MaximumNumberOfPlayersInARoom,
				PlayerTtl = _TimeBeforeRemovingPlayerAfterDisconnect,
				PublishUserId = true
			};

			if (roomName.Length > 1)
			{
				PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
			}
			else
			{
				Debug.LogWarningFormat("Enter valid room name. Room name should have more than one character");
			}
		}

		public void LeaveGame()
		{
			PhotonNetwork.LeaveRoom();
			Debug.LogFormat("Leaving room...");
		}

	#endregion

	#region Properties

		public int MaximumNumberOfPlayersInARoom
		{
			get
			{
				return _MaximumNumberOfPlayersInARoom;
			}
		}

		public int TimeBeforeRemovingPlayerAfterDisconnect
		{
			get
			{
				return _TimeBeforeRemovingPlayerAfterDisconnect;
			}
		}

	#endregion
}
