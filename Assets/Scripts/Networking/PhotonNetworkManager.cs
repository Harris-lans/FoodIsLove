﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
	#region Global Variables

		[SerializeField]
        private string _DefaultRoomName = "ROOM-";

		public static PhotonNetworkManager Instance;

		[Header("Network Details")]
		[SerializeField]
		private string _GameVersion = "v0.1";

		[Space,Header("Room Information")]
		[SerializeField]
		private byte _MaximumNumberOfPlayersInARoom = 2;
		[SerializeField]
		private int _TimeBeforeRemovingPlayerAfterDisconnect = 2000;

        public bool InRoom {get; private set;}
		private string _RoomName;

		#region Photon Network Events

			[Header("Photon Network Events")]
			public UnityEvent OnConnectedToMasterEvent;
			public UnityEvent OnLocalPlayerJoinedRoomEvent;
			public UnityEvent OnCreatedRoomEvent;
			public UnityEvent OnLocalPlayerLeftRoomEvent;
			public UnityEvent OnDisconnectedFromPhotonEvent;
			public UnityEvent OnRemotePlayerJoinedRoomEvent;
	        public UnityEvent OnPlayerFailedToJoinRoomEvent;
			public UnityEvent OnRemotePlayerLeftRoomEvent;	

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

		public override void OnConnectedToMaster()
		{
			OnConnectedToMasterEvent.Invoke();
		}

		public override void OnJoinedRoom()
		{
			InRoom = true;
			OnLocalPlayerJoinedRoomEvent.Invoke();
		}

		public override void OnCreatedRoom()
		{
			OnCreatedRoomEvent.Invoke();
		}

		public override void OnLeftRoom()
		{
			InRoom = false;
			OnLocalPlayerLeftRoomEvent.Invoke();
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			Debug.LogFormat("Cause of Disconnection: {0}", cause);
			OnDisconnectedFromPhotonEvent.Invoke();
		}

		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			OnRemotePlayerJoinedRoomEvent.Invoke();
		}

		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
			OnRemotePlayerLeftRoomEvent.Invoke();
		}

	#endregion

	#region Member Functions

		public void CreateGame()
		{
            Debug.Log("Creating a room");
		    StartCoroutine(TryToCreateRoom());
        }

        public void CreateOrJoinRandomGame()
        {
            bool joinedRandomRoom = PhotonNetwork.JoinRandomRoom();
            if (joinedRandomRoom)
            {
                OnPlayerFailedToJoinRoomEvent.Invoke();
                StartCoroutine(TryToCreateRoom());
            }
            else
            {
                Debug.Log("Joined room successfully");
            }
        }

        private IEnumerator TryToCreateRoom()
        {
            bool createdRoom = false;
            int roomIndex = 0;
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = _MaximumNumberOfPlayersInARoom,
                PlayerTtl = _TimeBeforeRemovingPlayerAfterDisconnect,
                PublishUserId = true
            };

            while (!createdRoom && !InRoom)
            {
                createdRoom = PhotonNetwork.CreateRoom(_DefaultRoomName + roomIndex, roomOptions);
                ++roomIndex;
                yield return null;
            }
        }

        public void LeaveGame()
		{
			PhotonNetwork.LeaveRoom();
            StopAllCoroutines();
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
