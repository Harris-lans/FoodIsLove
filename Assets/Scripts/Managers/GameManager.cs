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
		
		[Header("Combat Data")]
		[SerializeField]
		private SO_CombatData _CombatData;

		[Header("UI Data")]
		[SerializeField]
		private SO_Tag _UIGameScreenTag;
		[SerializeField]
		private SO_Tag _UIGameOverTag;
		[SerializeField]
		private SO_Tag _UICombatScreenTag;

		private UIManager _UIManager;
		private PhotonView _PhotonView;

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
            _UIManager.SetScreen(_UIGameScreenTag);
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
		private void StartGame()
		{
			_MatchState.MatchStarted = true;
			StartGameEvent.Invoke();
			_LobbyDetails.Judge.AnnouncementEvent.Invoke(null);
			_GameStartedEvent.Invoke(null);
		}

		private void OnPlayerCompletedAllDishes(object playerId)
		{
			int playerViewId = (int) playerId;
			APlayerController player = PhotonView.Find(playerViewId).GetComponent<APlayerController>();
			// TODO: Store winner's name in scriptable object

			Debug.Log("Player won the match");
			_UIManager.SetScreen(_UIGameOverTag);
		}

		private void OnHeroesCollided(object data)
		{
			// Showing the combat UI
			_UIManager.SetScreen(_UICombatScreenTag);
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
			_PhotonView.RPC("StartGame", RpcTarget.All);
		}

	#endregion

	#region Network Callbacks



	#endregion
}

public enum NetworkedGameEvents : byte
{
	START_MATCH = 0,
	END_MATCH,
	SPAWNED_HERO,
	SPAWNED_MINION,
	SPAWNED_PLAYERCONTROLLER,
	ON_SELECTED_INGREDIENT,
	ON_SELECTED_COOKING_STATION,
	ON_SELECTED_NODE,
    ON_SELECTED_COMBAT_OPTION,
    ON_COMBAT_SEQUENCE_STARTED,
    ON_COMBAT_SEQUENCE_RESTARTED,
    ON_COMBAT_SEQUENCE_ENDED,
    ON_COMBAT_SEQUENCE_RESULT,
	ON_HEROES_COLLIDED_EVENT
}
