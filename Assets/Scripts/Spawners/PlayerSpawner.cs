using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using LLAPI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour 
{
	#region Member Variables

		[Header("Combat Data")]
		[SerializeField]
		private SO_CombatData _CombatData;

		[Header("Spawn Details")]
		private float _HeroRespawnTime = 3.0f; 

		private SO_LevelData _LevelData;
		private SO_LobbyDetails _LobbyDetails;
        private GridSystem _GridSystem;
		private bool _CanSpawn;
		private LocalPlayerController _LocalPlayerController; 
		private Dictionary<int, NetPlayerController> _NetPlayerControllers;

	#endregion

	#region Life Cycle

		private void OnEnable() 
		{
			PhotonNetwork.NetworkingClient.EventReceived += OnNetworkEvent;
			_CombatData.LocalHeroKilledEvent.AddListener(OnLocalHeroKilled);
		}

		private void OnDisable()
		{
			PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEvent;
			_CombatData.LocalHeroKilledEvent.RemoveListener(OnLocalHeroKilled);
		}

		private void Awake()
		{
			_NetPlayerControllers = new Dictionary<int, NetPlayerController>();
		    _GridSystem = GridSystem.Instance;

			if(PhotonNetwork.IsMasterClient)
			{
				_CanSpawn = true;
			}
		}

	#endregion

	#region Member Functions

		#region Locally called functions

			public void SpawnPlayer(SO_LobbyDetails lobbyDetails)
			{
				_LobbyDetails = lobbyDetails;
				_LocalPlayerController = SpawnPlayerController();

				if (PhotonNetwork.IsMasterClient)
				{
					SpawnHero(GetFreeSpawnPoint());
				}
				else
				{
					RequestSpawnPoint();
				}
		    }

			private LocalPlayerController SpawnPlayerController()
			{
				// Spawning the PlayerController on all the clients
				var playerControllerObject = PhotonNetwork.Instantiate("PlayerController", Vector3.zero, Quaternion.identity);
				var localPlayerController = playerControllerObject.AddComponent<LocalPlayerController>();
				PhotonView view = playerControllerObject.GetComponent<PhotonView>();

				// Raising Net Event
				RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };
				SendOptions sendOptions = new SendOptions { Reliability = true };
				PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.SPAWNED_PLAYERCONTROLLER, view.ViewID, eventOptions, sendOptions);

				return localPlayerController;
			}

			private void SpawnHero(GridPosition chosenSpawnPoint)
			{
				string heroPrefabName = _LobbyDetails.ChosenHero.HeroPrefab.name;
				int[] viewIDs = new int[2];

				// Spawning the hero on all the clients
				GameObject spawnedHero = PhotonNetwork.Instantiate(heroPrefabName, _GridSystem.GetActualCoordinates(chosenSpawnPoint), Quaternion.identity);
				var view = spawnedHero.GetComponent<PhotonView>();
				HeroController heroController = spawnedHero.GetComponent<HeroController>();

				// Sending the view id of the player controller as well
				viewIDs[0] = _LocalPlayerController.GetComponent<PhotonView>().ViewID;
				viewIDs[1] = view.ViewID;

				// Raising Net Event
				RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };
				SendOptions sendOptions = new SendOptions { Reliability = true };
				PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.SPAWNED_HERO, viewIDs, eventOptions, sendOptions);

				// Initializing the local player controller
				_LocalPlayerController.Initialize(heroController);
			}

			private void RequestSpawnPoint()
			{
				Byterizer byterizer = new Byterizer();
				byterizer.Push(PhotonNetwork.LocalPlayer.UserId);
				byte[] data = byterizer.GetBuffer();

				// Raising Net Event
				RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };
				SendOptions sendOptions = new SendOptions { Reliability = true };
				PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.REQUEST_SPAWN_POINT, data, eventOptions, sendOptions);
			}
			
			private GridPosition GetFreeSpawnPoint()
			{
				GridPosition selectedGridPosition;

				HeroSpawner[] heroSpawners = GameObject.FindObjectsOfType<HeroSpawner>();
				
				HeroSpawner chosenHeroSpawner = null;
				System.Random random = new System.Random((int)Time.timeSinceLevelLoad);

				while(chosenHeroSpawner == null)
				{
					var heroSpawner = heroSpawners[random.Next(0, heroSpawners.Length)];
					if(!heroSpawner.IsOccupied)
					{
						chosenHeroSpawner = heroSpawner;
					}
				}

				selectedGridPosition = _GridSystem.GetGridPosition(chosenHeroSpawner.transform.position);

				return selectedGridPosition;
			}

			private IEnumerator RespawnTimer()
			{
				yield return new WaitForSeconds(_HeroRespawnTime);

				if (PhotonNetwork.IsMasterClient)
				{
					SpawnHero(GetFreeSpawnPoint());
				}
				else
				{
					RequestSpawnPoint();
				}
			}

			private void OnLocalHeroKilled(object data)
			{
				// Starting Player Respawn
				StartCoroutine(RespawnTimer());
			}

		#endregion

	#endregion

	#region Network Callbacks

		private void OnNetworkEvent(EventData eventData)
		{
			NetworkedGameEvents eventCode = (NetworkedGameEvents)eventData.Code;
			object data = eventData.CustomData;

			if (eventCode == NetworkedGameEvents.SPAWNED_PLAYERCONTROLLER)
			{
				RegisterPlayerControllerSpawnedOnRemoteClient((int)data);
			}
			else if (eventCode == NetworkedGameEvents.SPAWNED_HERO)
			{
				RegisterHeroSpawnedOnRemoteClient((int[])data);
			}
			else if (eventCode == NetworkedGameEvents.REQUEST_SPAWN_POINT)
			{
				OnRequestedSpawnPoint((byte[])data);
			}
			else if (eventCode == NetworkedGameEvents.SEND_SPAWN_POINT)
			{
				OnRecievedSpawnPoint((byte[])data);
			}
		}

		private void RegisterPlayerControllerSpawnedOnRemoteClient(int viewID)
		{
			PhotonView playerControllerView = PhotonView.Find(viewID);
			NetPlayerController netPlayerController = playerControllerView.gameObject.AddComponent<NetPlayerController>();
			_NetPlayerControllers[viewID] = netPlayerController;
		}

		private void RegisterHeroSpawnedOnRemoteClient(int[] viewIDs)
		{
			if (_NetPlayerControllers.ContainsKey(viewIDs[0]))
			{
				PhotonView viewOfHeroObject = PhotonView.Find(viewIDs[1]);
				HeroController heroController = viewOfHeroObject.GetComponent<HeroController>();
				_NetPlayerControllers[viewIDs[0]].Initialize(heroController);
			}
			else
			{
				Debug.Log("PlayerController not found");
			}
		}

		private void OnRequestedSpawnPoint(byte[] data)
		{
			// Only the master client can provide spawn points
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}

			Byterizer byterizer = new Byterizer();
			byterizer.LoadDeep(data);

			GridPosition gridPosition = GetFreeSpawnPoint();

			byterizer.Push(gridPosition.X);
			byterizer.Push(gridPosition.Z);

			data = byterizer.GetBuffer();

			// Raising Net Event
			RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };
			SendOptions sendOptions = new SendOptions { Reliability = true };
			PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.SEND_SPAWN_POINT, data, eventOptions, sendOptions);
		}

		private void OnRecievedSpawnPoint(byte[] data)
		{
			Byterizer byterizer = new Byterizer();
			byterizer.LoadDeep(data);

			// Checking if this spawn point is met for this player
			string playerID = byterizer.PopString();

			if (playerID != PhotonNetwork.LocalPlayer.UserId)
			{
				return;
			}

			GridPosition chosenHeroSpawn = new GridPosition(byterizer.PopByte(), byterizer.PopByte());
			SpawnHero(chosenHeroSpawn);
		}

	#endregion
}
