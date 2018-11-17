using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartScreen : UIScreen 
{
	#region Member Variables

		[SerializeField]
		private SO_LobbyDetails _LobbyDetails;

		[Space, Header("UI Elements")]
		[SerializeField]
		private Image _DishImage;
		[SerializeField]
		private Text _DishName;

	#endregion

	#region Life Cycle

		private void OnEnable()
		{
			ShowDishDetails();
		}

	#endregion

	#region Member Functions

		private void ShowDishDetails()
		{
			// FIXME: For now there is only one dish per match
			_DishImage.sprite = _LobbyDetails.ChosenDishes[0].DishThumbnail;
			_DishName.text = _LobbyDetails.ChosenDishes[0].DishName;
		}

	#endregion
}
