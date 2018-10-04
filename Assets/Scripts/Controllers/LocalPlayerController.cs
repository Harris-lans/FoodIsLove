using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using LLAPI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LocalPlayerController : APlayerController
{
	#region Member Variables

		// Networking Variables
		private RaiseEventOptions _RaiseEventOptions;
		private SendOptions _SendOptions;

		[Header("Events")]
		private SO_GenericEvent _CookingStationPopUpClickedEventHandler;
		private SO_GridSelectEventHandler _GridSelectEventHandler;

		private SO_MatchState _MatchState;

	#endregion

	#region Life Cycle

		protected override void Start()
		{
			base.Start();

			// Initializing Network variables
			_RaiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
			_SendOptions = new SendOptions { Reliability = true };

			// Loading required level data
			SO_LevelData levelData = Resources.Load<SO_LevelData>("CurrentLevelData");

			// Subscribing to events
			levelData.GridSelectEventHandler.SubscribeToGridSelectEvent(OnSelectedGridCell);
			levelData.IngredientSelectEventHandler.AddListener(OnSelectedIngredient);
			levelData.CookingStationPopUpClickedEventHandler.AddListener(OnSelectedCookingStationPopUp);
            
            // Subscribing to combat sequences
			_CombatData.HeroesCollidedEvent.AddListener(OnHeroesCollidedEvent);
            _CombatData.CombatSequenceRestartedEvent.AddListener(OnCombatSequenceRestarted);
            _CombatData.CombatSequenceCompletedEvent.AddListener(OnCombatSequenceEnded);
            _CombatData.CombatOptionChosenEvent.AddListener(OnCombatOptionSelected);
			_CombatData.ShowCombatResultsEvent.AddListener(OnShowCombatResults);

			// Storing Match State
			_MatchState = levelData.MatchState;
		}

	#endregion

	#region Member Functions

		public override void Initialize(HeroController hero)
		{
			base.Initialize(hero);
			hero.Initialize(true);
		}

		private void OnSelectedGridCell(GridPosition selectedCell, GridProp selectedObject)
		{
			if (!_MatchState.MatchStarted || _HeroCharacter.IsInCombat)
			{
				return;
			}

			// Only react if the selected spot is a node
			if (selectedObject != null)
			{
				var node = selectedObject.GetComponent<ANode>();
				if (node != null)
				{
					OnSelectedNode(selectedCell, node);
				}
			}
		}

		private void OnSelectedCookingStationPopUp(object cookingStationData)
		{
			ANode cookingStation = (ANode) cookingStationData;
			GridPosition gridPosition = _GridSystem.GetGridPosition(cookingStation.transform.position);
			OnSelectedNode(gridPosition, cookingStation);
		}

		protected override void OnSelectedIngredient(object selectedIngredient)
		{
			base.OnSelectedIngredient(selectedIngredient);

			IngredientMinion ingredient = (IngredientMinion)selectedIngredient;
			var ingredientView = ingredient.GetComponent<PhotonView>();

			// Preparing payload
			Byterizer byterizer = new Byterizer();
			byterizer.Push(_PhotonView.ViewID);
			byterizer.Push(ingredientView.ViewID);
			byte[] data = byterizer.GetBuffer();

			// Raising Net Event
			PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_SELECTED_INGREDIENT, data, _RaiseEventOptions, _SendOptions);
		}

		protected override void OnSelectedNode(GridPosition selectedCell, ANode node)
		{
			base.OnSelectedNode(selectedCell, node);

			var nodeView = node.GetComponent<PhotonView>();
			if (nodeView == null)
			{
				return;
			}

			// Preparing payload
			Byterizer byterizer = new Byterizer();
			byterizer.Push(_PhotonView.ViewID);
			byterizer.Push(nodeView.ViewID);
			byterizer.Push(selectedCell.X);
			byterizer.Push(selectedCell.Z);
			byte[] data = byterizer.GetBuffer();

            Debug.Log("Moving to a node");

			// Raising Net Event
			PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_SELECTED_NODE, data, _RaiseEventOptions, _SendOptions);
		}

		private void OnHeroesCollidedEvent(object data)
		{
			Debug.LogFormat("Net Event Raised: {0}", NetworkedGameEvents.ON_HEROES_COLLIDED_EVENT);
			// Raising Net Event (No data required in this case)
            PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_HEROES_COLLIDED_EVENT, null, _RaiseEventOptions, _SendOptions);
		}

        private void OnCombatSequenceRestarted(object data)
        {
			Debug.LogFormat("Net Event Raised: {0}", NetworkedGameEvents.ON_COMBAT_SEQUENCE_RESTARTED);
            // Raising Net Event (No data required in this case)
            PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_COMBAT_SEQUENCE_RESTARTED, null, _RaiseEventOptions, _SendOptions);
        }

		private void OnShowCombatResults(object results)
		{
			// Only the master client sends the flag to show reults
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}

			Debug.LogFormat("Net Event Raised: {0}", NetworkedGameEvents.ON_COMBAT_SEQUENCE_RESULT);
			// Telling the other clients to show the results
			int[] combatData = (int[]) results; 

            Byterizer byterizer = new Byterizer();
            byterizer.Push(combatData[0]);
            byterizer.Push((byte)combatData[1]);

            byte[] data = byterizer.GetBuffer();

            // Raising Net Event
            PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_COMBAT_SEQUENCE_RESULT, data, _RaiseEventOptions, _SendOptions);
		}

        private void OnCombatSequenceEnded(object data)
        {
			// Killing the player who lost
			if (_PhotonView.ViewID != (int)data)
			{
				_CombatData.LocalHeroKilledEvent.Invoke(null);
				_HeroCharacter.Kill();
			}

			// Only the master client can sequence ended
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}

			Debug.LogFormat("Net Event Raised: {0}", NetworkedGameEvents.ON_COMBAT_SEQUENCE_ENDED);
            // Raising Net Event
            PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_COMBAT_SEQUENCE_ENDED, (int)data, _RaiseEventOptions, _SendOptions);
        }

        private void OnCombatOptionSelected(object combatOptions)
        {
			Debug.LogFormat("Net Event Raised: {0}", NetworkedGameEvents.ON_SELECTED_COMBAT_OPTION);
            int[] combatData = (int[]) combatOptions; 

            Byterizer byterizer = new Byterizer();
            byterizer.Push(combatData[0]);
            byterizer.Push((byte)combatData[1]);

            byte[] data = byterizer.GetBuffer();

            // Raising Net Event (No data required in this case)
            PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_SELECTED_COMBAT_OPTION, data, _RaiseEventOptions, _SendOptions);
        }

    #endregion
}
