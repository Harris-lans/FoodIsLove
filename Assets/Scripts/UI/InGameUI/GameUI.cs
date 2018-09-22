using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIScreen 
{
	#region Global Variables

		[Header("Game Progression Items")]
		[SerializeField]
		private Slider _LocalPlayerSlider;
		[SerializeField]
		private Slider _RemotePlayerSlider;

		[Header("Events to listen to")]
		[SerializeField]
		private SO_GenericEvent _DishCookedEvent;

		[Header("Match Stats")]
		[SerializeField]
		private SO_MatchState _MatchState;

	#endregion

	#region Life Cycle

		protected override void Start() 
		{
			base.Start();

			// Subscribing to dish cooked events of all players
			_DishCookedEvent.AddListener(OnDishCooked);

			// Initializing the UI
			_LocalPlayerSlider.value = 0;
			_RemotePlayerSlider.value = 0;
		}

	#endregion

	#region Member Functions

		private void UpdateUI(Slider progressSlider, CookingPot cookingPot)
		{
			progressSlider.value = cookingPot.CurrentDishStatusFraction;
		}

		private void OnDishCooked(object playerViewID)
		{
			int viewID = (int)playerViewID;
			var localPlayer = PhotonView.Find(viewID).GetComponent<LocalPlayerController>();

			if (localPlayer == null)
			{
				UpdateUI(_RemotePlayerSlider, _MatchState.PlayerCookingPots[viewID]);
			}
			else
			{
				UpdateUI(_LocalPlayerSlider, _MatchState.PlayerCookingPots[viewID]);
			}
		}

	#endregion
}