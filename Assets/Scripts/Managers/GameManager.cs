using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBehaviour<GameManager>
{
	#region Member Variables

		[Header("Match Details")]
		[SerializeField]
		private SO_LobbyDetails _LobbyDetails;
		[SerializeField]
		private SO_MatchState _MatchState;
        [SerializeField]
        private SO_IngredientSpawnData _IngredientSpawnData;

		[Header("Events")]
		public UnityEvent StartGameEvent;
		[SerializeField]
		private SO_GenericEvent _AllDishesCookedEvent;
		[SerializeField]
		private SO_GenericEvent _GameStartedEvent;
		[SerializeField]
		private SO_GenericEvent _GameEndedEvent;
		
		[Header("Combat Data")]
		[SerializeField]
		private SO_CombatData _CombatData;

		[Header("UI Data")]
		[SerializeField]
		private SO_Tag _GameStartScreen;
		[SerializeField]
		private SO_Tag _GameTimerScreen;
		[SerializeField]
		private SO_Tag _UIGameScreenTag;
		[SerializeField]
		private SO_Tag _UIGameOverTag;
		[SerializeField]
		private SO_Tag _UIClashScreenTag;

		private UIManager _UIManager;
		private PhotonView _PhotonView;
		private PhotonNetworkManager _PhotonNetworkManager;

	#endregion

	#region LifeCycle

		override protected void SingletonAwake()
		{
			// TODO: Get the match time
			_MatchState.Initialize(0, _LobbyDetails.ChosenDishes);
			_AllDishesCookedEvent.Initialize();
		    _IngredientSpawnData.Initialize();

		    foreach (var dish in _LobbyDetails.ChosenDishes)
		    {
                // Adding the ingredient twice because both the players are cooking the same ingredient
		        _IngredientSpawnData.AddRecipeIngredientsForSpawning(dish.DishRecipe);
		        _IngredientSpawnData.AddRecipeIngredientsForSpawning(dish.DishRecipe);
		    }

			_UIManager = UIManager.Instance;
			_PhotonView = GetComponent<PhotonView>();

			// Can start game only if the client is the master client
			if (PhotonNetwork.IsMasterClient)
			{
				StartCoroutine(CheckIfCanStartGame());
			}

			// Subscribing to player cooked all dishes event
			_AllDishesCookedEvent.AddListener(OnPlayerCompletedAllDishes);

			_CombatData = Resources.Load<SO_CombatData>("CombatData");

			// Subscribing to combat events
			_CombatData.HeroesCollidedEvent.AddListener(OnHeroesCollided);	
			_CombatData.CombatSequenceCompletedEvent.AddListener(OnCombatSequenceEnded);	
		}

		override protected void SingletonStart()
		{
			StartCoroutine(CheckIfCanSpawnPlayerController());
            _UIManager.SetScreen(_GameStartScreen);

			// Subscribing to PhotonNetwork Event
			_PhotonNetworkManager = PhotonNetworkManager.Instance;
			_PhotonNetworkManager.OnRemotePlayerLeftRoomEvent.AddListener(OnPlayerDroppedOut);
		}

	#endregion

	#region Member Functions

		private void SpawnPlayers()
		{
			PlayerSpawner playerSpawner = GetComponent<PlayerSpawner>();

			if (playerSpawner != null)
			{
				playerSpawner.SpawnPlayer(lobbyDetails: _LobbyDetails);
			}
			else
			{
				Debug.Log("Player Spawner not detected. Make sure you attach the spawner to the Game Manager!");
			}
		}

		[PunRPC]
		private void StartMatchTimer()
		{
			_UIManager.SetScreen(_GameTimerScreen);
			StartCoroutine(StartGame());
		}

		private IEnumerator StartGame()
		{
			while(_UIManager.CurrentScreen.UIScreenTag == _GameTimerScreen)
			{
				yield return null;
			}

			_MatchState.MatchStarted = true;
			StartGameEvent.Invoke();
			_LobbyDetails.Judge.AnnouncementEvent.Invoke(null);
			_GameStartedEvent.Invoke(null);
		}

		private void OnPlayerCompletedAllDishes(object playerId)
		{
			int playerViewId = (int) playerId;

			_MatchState.MatchOver = true;
			_MatchState.WonTheMatch = PhotonView.Find(playerViewId).IsMine;
			_MatchState.GameOverReason = GameOverReason.DISH_COMPLETED_BY_SOMEONE;
			_PhotonNetworkManager.LeaveGame();
			_GameEndedEvent.Invoke(null);
			_UIManager.SetScreen(_UIGameOverTag);
		}

		private void OnHeroesCollided(object data)
		{
			// Showing the combat UI
			_UIManager.SetScreen(_UIClashScreenTag);
			_CombatData.CombatSequenceStartedEvent.Invoke(null);
		}

		private void OnCombatSequenceEnded(object data)
		{
			// Showing the game UI
			_UIManager.SetScreen(_UIGameScreenTag);
		}

	#endregion

	#region Co-Routines

		private IEnumerator CheckIfCanSpawnPlayerController()
		{
			while (PhotonNetwork.LevelLoadingProgress != 0)
			{
				Debug.Log("Checking if playercontroller can be spawned!");
				yield return null;
			}

			// Waiting for all the clients to load the level and then asking them to spawn the players
			SpawnPlayers();
		}

		private IEnumerator CheckIfCanStartGame()
		{
			bool playersJoining = true;
			Debug.Log("Waiting for all players...");

			while(playersJoining)
			{
				APlayerController[] playerControllers = FindObjectsOfType<APlayerController>();

				// Checking if all players have their PlayerControllers spawned
				if (playerControllers.Length >= _LobbyDetails.MaximumPlayersAllowed)
				{
					playersJoining = false;
				}
				yield return null;
			}
            
            yield return new WaitForSeconds(3);
			Debug.Log("Starting Game...");
			_PhotonView.RPC("StartMatchTimer", RpcTarget.All);
		}

	#endregion

	#region Network Callbacks

		private void OnPlayerDroppedOut()
		{
			// Only completing the match if there is only one player in the room
			if (PhotonNetwork.CurrentRoom.PlayerCount != 1 || _MatchState.MatchOver)
			{
				return;
			}

			// Player dropped from match
			_MatchState.MatchOver = true;
			_MatchState.WonTheMatch = true;
			_MatchState.GameOverReason = GameOverReason.PLAYER_DROPPED;
			_PhotonNetworkManager.LeaveGame();
			_UIManager.SetScreen(_UIGameOverTag);
		}

	#endregion
}

public enum NetworkedGameEvents : byte
{
	START_MATCH = 0,
	END_MATCH,
	REQUEST_SPAWN_POINT,
	SEND_SPAWN_POINT,
	SPAWNED_HERO,
	SPAWNED_MINION,
	PICKED_UP_INGREDIENT,
	SPAWNED_PLAYERCONTROLLER,
	ON_SELECTED_INGREDIENT,
	ON_SELECTED_COOKING_STATION,
	ON_SELECTED_NODE,
    ON_SELECTED_COMBAT_OPTION,
    ON_COMBAT_SEQUENCE_STARTED,
	ON_START_COMBAT_TIMER,
    ON_COMBAT_SEQUENCE_RESTARTED,
    ON_COMBAT_SEQUENCE_ENDED,
    ON_COMBAT_SEQUENCE_RESULT,
	ON_HEROES_COLLIDED_EVENT
}

public enum GameResult : byte
{
	LOST,
	WON
}
