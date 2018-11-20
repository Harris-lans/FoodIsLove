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
		[SerializeField]
		private float _TimeBeforeShowingStartScreen = 5.0f;

		[Header("Global Events")]
		public UnityEvent StartGameEvent;
		[SerializeField]
		private SO_GenericEvent _AllDishesCookedEvent;
		[SerializeField]
		private SO_GenericEvent _EnteredMatchEvent;
		[SerializeField]
		private SO_GenericEvent _GameStartedEvent;
		[SerializeField]
		private SO_GenericEvent _GameEndedEvent;
		[SerializeField]
		private SO_GenericEvent _MatchStoppedEvent;
		[SerializeField]
		private SO_GenericEvent _LocalPlayerWonEvent;
		[SerializeField]
		private SO_GenericEvent _LocalPlayerLostEvent;
		
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
		private SO_MatchStartTimings _MatchStartTimings;
		private SO_MatchEndTimings  _MatchEndTimings;
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
			_MatchStartTimings = Resources.Load<SO_MatchStartTimings>("MatchStartTimings");
			_MatchEndTimings = Resources.Load<SO_MatchEndTimings>("MatchEndTimings");

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
			_EnteredMatchEvent.Invoke(null);

			// Subscribing to PhotonNetwork Event
			_PhotonNetworkManager = PhotonNetworkManager.Instance;
			_PhotonNetworkManager.OnRemotePlayerLeftRoomEvent.AddListener(OnPlayerDroppedOut);
		}

		private void OnDestroy()
		{
			_CombatData.HeroesCollidedEvent.RemoveListener(OnHeroesCollided);	
			_CombatData.CombatSequenceCompletedEvent.RemoveListener(OnCombatSequenceEnded);
			_PhotonNetworkManager.OnRemotePlayerLeftRoomEvent.RemoveListener(OnPlayerDroppedOut);
		}

	#endregion

	#region Member Functions

		[PunRPC]
		private void StartMatch()
		{
			StartCoroutine(StartGame());
		}

		private void SpawnPlayers()
		{
			PlayerSpawner playerSpawner = GetComponent<PlayerSpawner>();

			if (playerSpawner != null)
			{
				playerSpawner.SpawnPlayer(_LobbyDetails);
			}
			else
			{
				Debug.Log("Player Spawner not detected. Make sure you attach the spawner to the Game Manager!");
			}
		}

		private void OnPlayerCompletedAllDishes(object playerId)
		{
			int playerViewId = (int) playerId;
			StartCoroutine(EndGame(PhotonView.Find(playerViewId).IsMine, GameOverReason.DISH_COMPLETED_BY_SOMEONE));
		}

		private void OnPlayerDroppedOut()
		{
			// Only completing the match if there is only one player in the room
			if (PhotonNetwork.CurrentRoom.PlayerCount != 1 || _MatchState.MatchOver)
			{
				return;
			}
			StartCoroutine(EndGame(true, GameOverReason.PLAYER_DROPPED));
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

		private IEnumerator CheckIfCanStartGame()
		{
			while (PhotonNetwork.LevelLoadingProgress != 1)
			{
				Debug.Log("Checking if playercontroller can be spawned!");
				yield return null;
			}

			Debug.Log("Starting Game...");
			_PhotonView.RPC("StartMatch", RpcTarget.All);			
		}

		private IEnumerator StartGame()
		{
			SpawnPlayers();

			yield return new WaitForSeconds(_MatchStartTimings.TimeDelayBeforeShowingMap);

			_UIManager.SetScreen(null);

			yield return new WaitForSeconds(_MatchStartTimings.TimeDelayBeforeDisplayingTheMatchCountDown);

			_UIManager.SetScreen(_GameTimerScreen);

			while(_UIManager.CurrentScreen.UIScreenTag == _GameTimerScreen)
			{
				yield return null;
			}

			yield return new WaitForSeconds(_MatchStartTimings.TimeDelayBeforeSpawningThePlayers);

			while(APlayerController.PlayerControllers.Count < PhotonNetwork.CurrentRoom.MaxPlayers)
			{
				yield return null;
			}

			// Spawning the ingredients and the player
			_MatchState.MatchStarted = true;

			_LobbyDetails.Judge.AnnouncementEvent.Invoke(null);
			_GameStartedEvent.Invoke(null);
		}

		private IEnumerator EndGame(bool wonTheMatch, GameOverReason gameOverReason)
		{
			_UIManager.SetScreen(null);

			_MatchState.MatchOver = true;
			_MatchState.MatchStarted = false;
			_MatchState.WonTheMatch = wonTheMatch;
			_MatchState.GameOverReason = gameOverReason;

			while(_UIManager.CurrentScreen != null)
			{
				yield return null;
			}

			_MatchStoppedEvent.Invoke(null);

			if (wonTheMatch)
			{
				_LocalPlayerWonEvent.Invoke(null);
			}
			else
			{
				_LocalPlayerLostEvent.Invoke(null);
			}

			yield return new WaitForSecondsRealtime(_MatchEndTimings.TimeBeforeTriggerinigSlowMotion);

			// Slowing Down Time
			float timeEstablished = _MatchEndTimings.SlowMotionTime;
			while(timeEstablished > 0)
			{
				yield return new WaitForSecondsRealtime(0.01f);
				timeEstablished -= 0.01f;
				Time.timeScale = Mathf.SmoothStep(Time.timeScale, 0.2f, timeEstablished / _MatchEndTimings.SlowMotionTime);
			}

			// Speeding up Time
			timeEstablished = _MatchEndTimings.TimeToComeBackToNormalSpeed;
			while(timeEstablished > 0)
			{
				yield return new WaitForSecondsRealtime(0.01f);
				timeEstablished -= 0.01f;
				Time.timeScale = Mathf.SmoothStep(Time.timeScale, 1, timeEstablished / _MatchEndTimings.SlowMotionTime);
			}

			_UIManager.SetScreen(_UIGameOverTag);
			_PhotonNetworkManager.LeaveGame();

			yield return new WaitForSecondsRealtime(_MatchEndTimings.TimeBeforeAnnouncing);
			_GameEndedEvent.Invoke(null);

			yield return null;
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
