using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour 
{
	#region Member Variables

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
		}

		private void OnDisable()
		{
			PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEvent;
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
				_LocalPlayerController = SpawnPlayerController();
				HeroController heroController = SpawnHero(lobbyDetails); 
				//List<LocalMinionController> minionControllers = SpawnMinions(lobbyDetails); 

				// Initializing the local player controller
				_LocalPlayerController.Initialize(heroController);

				// TODO: Now pairing of minions with their respective button only happens when the hero goes to their position
				// // Connecting the buttons to their respective minion 
				// SO_GameUIData uiData = Resources.Load<SO_GameUIData>("Default_GameUIData");
				// uiData.HeroSlot.Hero = heroController;
				// uiData.MinionSlot1.Minon = minionControllers[0];
				// uiData.MinionSlot2.Minon = minionControllers[1];
				// uiData.MinionSlot3.Minon = minionControllers[2];
				// uiData.MinionSlot4.Minon = minionControllers[3];

				// Connecting the buttons to their respective skill
				//uiData.PrimarySkillSlot.Skill = heroController.Skills[ASkill.SkillTag.PRIMARY_ACTIVE];
				//uiData.SecondarySkillSlot.Skill = heroController.Skills[ASkill.SkillTag.SECONDARY_ACTIVE];
				//uiData.PassiveSkillSlot.Skill = heroController.Skills[ASkill.SkillTag.PASSIVE];
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

			private HeroController SpawnHero(SO_LobbyDetails lobbyDetails)
			{
				// Currently randomizing spawn points
				GridPosition chosenSpawnPoint = GetFreeSpawnPoint();
				string heroPrefabName = lobbyDetails.ChosenHero.HeroPrefab.name;
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

				while (true)
				{
					int selectedRow = Random.Range(0, rows);
					int selectedColumn = Random.Range(0, cols);
					selectedGridPosition = new GridPosition((byte)selectedRow, (byte)selectedColumn);

					if (_GridSystem.GridData[selectedRow, selectedColumn] == null)
					{
						break;
					}
				}

				return selectedGridPosition;
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
