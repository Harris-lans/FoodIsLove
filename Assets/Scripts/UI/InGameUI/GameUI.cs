using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIScreen 
{
	#region Global Variables

		[Header("Objective Details")]
		[SerializeField]
		private Image _JudgeImage;
		[SerializeField]
		private Image _DishImage;

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

		private SO_LobbyDetails _LobbyDetails;

	#endregion

	#region Life Cycle

		private void Start() 
		{
			// Subscribing to dish cooked events of all players
			_DishCookedEvent.AddListener(OnDishCooked);

			// Initializing the UI
			_LocalPlayerSlider.value = 0;
			_RemotePlayerSlider.value = 0;
			var currentLevelData = Resources.Load<SO_LevelData>("CurrentLevelData");
			_LobbyDetails = currentLevelData.LobbyDetails;
			
			// Showing the judge photo and the target dish
			_JudgeImage.sprite = _LobbyDetails.Judge.JudgeThumbnail;
			
			// FIXME: This is temporary as now there is considered to be only one dish
			_DishImage.sprite = _LobbyDetails.ChosenDishes[0].DishThumbnail; 
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