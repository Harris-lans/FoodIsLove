using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
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
		}

	#endregion

	#region Member Functions

		#region Locally called functions

			public void SpawnPlayer(SO_LobbyDetails lobbyDetails)
			{
				_LobbyDetails = lobbyDetails;
				_LocalPlayerController = SpawnPlayerController();
				SpawnHero();
		    }

			private LocalPlayerController SpawnPlayerController()
			{
				// Spawning the PlayerController on all the objects
				var playerControllerObject = PhotonNetwork.Instantiate("PlayerController", Vector3.zero, Quaternion.identity);
				var localPlayerController = playerControllerObject.AddComponent<LocalPlayerController>();
				PhotonView view = playerControllerObject.GetComponent<PhotonView>();

				// Raising Net Event
				RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };
				SendOptions sendOptions = new SendOptions { Reliability = true };
				PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.SPAWNED_PLAYERCONTROLLER, view.ViewID, eventOptions, sendOptions);

				return localPlayerController;
			}

			private void SpawnHero()
			{
				HeroController heroController = InstantiateHero();  

				// Initializing the local player controller
				_LocalPlayerController.Initialize(heroController);
			}

			private HeroController InstantiateHero()
			{
				// Currently randomizing spawn points
				GridPosition chosenSpawnPoint = GetFreeSpawnPoint();

				// 99, 99 is considered invalid
				if (new GridPosition(99, 99) == chosenSpawnPoint)
				{
					Debug.Log("No spawn points are free");
					return null;
				}

				string heroPrefabName = _LobbyDetails.ChosenHero.HeroPrefab.name;
				int[] viewIDs = new int[2];

				// Spawning the hero on all the clients
				GameObject spawnedHero = PhotonNetwork.Instantiate(heroPrefabName, _GridSystem.GetActualCoordinates(chosenSpawnPoint), Quaternion.identity);
				var view = spawnedHero.GetComponent<PhotonView>();
				HeroController hero = spawnedHero.GetComponent<HeroController>();

				// Sending the view id of the player controller as well
				viewIDs[0] = _LocalPlayerController.GetComponent<PhotonView>().ViewID;
				viewIDs[1] = view.ViewID;

				// Raising Net Event
				RaiseEventOptions eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };
				SendOptions sendOptions = new SendOptions { Reliability = true };
				PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.SPAWNED_HERO, viewIDs, eventOptions, sendOptions);

				return hero;
			}
		
			private GridPosition GetFreeSpawnPoint()
			{
				int rows = _GridSystem.GridData.GetLength(0);
				int cols = _GridSystem.GridData.GetLength(1);
				
				GridPosition selectedGridPosition;

				HeroSpawner[] heroSpawners = GameObject.FindObjectsOfType<HeroSpawner>();

				foreach(var heroSpawner in heroSpawners)
				{
					if(!heroSpawner.IsOccupied)
					{
						selectedGridPosition = _GridSystem.GetGridPosition(heroSpawner.transform.position);
						return selectedGridPosition;
					}
				}

				return new GridPosition(99, 99);
			}

			private IEnumerator RespawnTimer()
			{
				yield return new WaitForSeconds(_HeroRespawnTime);
				SpawnHero();
			}

			private void OnLocalHeroKilled(object data)
			{
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

	#endregion
}
