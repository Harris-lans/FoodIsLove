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
		private SO_GenericEvent _StartCombatTimerEvent;

		private SO_MatchState _MatchState;
		private SO_LevelData _LevelData;

	#endregion

	#region Life Cycle

		protected override void Start()
		{
			base.Start();

			// Initializing Network variables
			_RaiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
			_SendOptions = new SendOptions { Reliability = true };

			// Loading required level data
			_LevelData = Resources.Load<SO_LevelData>("CurrentLevelData");

			// Subscribing to events
			_LevelData.IngredientSelectEventHandler.AddListener(OnSelectedIngredient);
			_LevelData.NodeClickedEventHandler.AddListener(OnSelectedANode);
            
            // Subscribing to combat sequences
			_CombatData.HeroesCollidedEvent.AddListener(OnHeroesCollidedEvent);
            _CombatData.CombatSequenceRestartedEvent.AddListener(OnCombatSequenceRestarted);
            _CombatData.CombatSequenceCompletedEvent.AddListener(OnCombatSequenceEnded);
            _CombatData.CombatOptionChosenEvent.AddListener(OnCombatOptionSelected);
			_CombatData.ShowCombatResultsEvent.AddListener(OnShowCombatResults);
			_CombatData.CombatTimerStartedEvent.AddListener(OnStartedCombatTimer);

			// Storing Match State
			_MatchState = _LevelData.MatchState;
		}

		private void OnDestroy()
		{
			// Subscribing to events
			_LevelData.IngredientSelectEventHandler.RemoveListener(OnSelectedIngredient);
			_LevelData.NodeClickedEventHandler.RemoveListener(OnSelectedANode);
            
            // Subscribing to combat sequences
			_CombatData.HeroesCollidedEvent.RemoveListener(OnHeroesCollidedEvent);
            _CombatData.CombatSequenceRestartedEvent.RemoveListener(OnCombatSequenceRestarted);
            _CombatData.CombatSequenceCompletedEvent.RemoveListener(OnCombatSequenceEnded);
            _CombatData.CombatOptionChosenEvent.RemoveListener(OnCombatOptionSelected);
			_CombatData.ShowCombatResultsEvent.RemoveListener(OnShowCombatResults);
			_CombatData.CombatTimerStartedEvent.RemoveListener(OnStartedCombatTimer);
		}

	#endregion

	#region Member Functions

		public override void Initialize(HeroController hero)
		{
			base.Initialize(hero);
			hero.Initialize(true);
		}

		private void OnSelectedANode(object data)
		{
			if (!_MatchState.MatchStarted || _HeroCharacter.IsInCombat)
			{
				return;
			}

			ANode node = (ANode) data;
			OnSelectedNode(node);
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

		protected override void OnSelectedNode(ANode node)
		{
			base.OnSelectedNode(node);

			var nodeView = node.GetComponent<PhotonView>();
			if (nodeView == null)
			{
				return;
			}

			// Preparing payload
			Byterizer byterizer = new Byterizer();
			byterizer.Push(_PhotonView.ViewID);
			byterizer.Push(nodeView.ViewID);
			byte[] data = byterizer.GetBuffer();

			// Raising Net Event
			PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_SELECTED_NODE, data, _RaiseEventOptions, _SendOptions);
		}

		private void OnHeroesCollidedEvent(object data)
		{
			// Raising Net Event (No data required in this case)
            PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_HEROES_COLLIDED_EVENT, null, _RaiseEventOptions, _SendOptions);
		}

        private void OnCombatSequenceRestarted(object data)
        {
            // Raising Net Event (No data required in this case)
            PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_COMBAT_SEQUENCE_RESTARTED, null, _RaiseEventOptions, _SendOptions);
        }

		private void OnStartedCombatTimer(object timeData)
		{
			float time = (float)timeData;
			Byterizer byterizer = new Byterizer();
			byterizer.Push(time);
			byte[] data = byterizer.GetBuffer();
			PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_START_COMBAT_TIMER, data, _RaiseEventOptions, _SendOptions);
		}

		private void OnShowCombatResults(object results)
		{
			// Only the master client sends the flag to show reults
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}

			// Telling the other clients to show the results
			int[] combatData = (int[]) results; 

            Byterizer byterizer = new Byterizer();
            byterizer.Push(combatData[0]);
            byterizer.Push((byte)combatData[1]);
			byterizer.Push(combatData[2]);

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

            // Raising Net Event
            PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_COMBAT_SEQUENCE_ENDED, (int)data, _RaiseEventOptions, _SendOptions);
        }

        private void OnCombatOptionSelected(object combatOptions)
        {
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
