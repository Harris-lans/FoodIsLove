﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIScreen 
{
	#region Global Variables

		[Header("UI Elements")]
		[SerializeField]
		private Image _DishImage;
		[SerializeField]
		private RectTransform _CookingStepsContainer;

		[Space, Header("UI Prefabs")]
		[SerializeField]
		private IngredientContainer _IngredientContainerPrefab;

		[Header("Game Progression Items")]
		[SerializeField]
		private Slider _LocalPlayerSlider;
		[SerializeField]
		private Slider _RemotePlayerSlider;

		[Space, Header("Events to listen to")]
		[SerializeField]
		private SO_GenericEvent _DishCookedEvent;
		[SerializeField]
		private SO_GenericEvent _MatchStartedEvent;

		[Space, Header("Match Stats and Data")]
		[SerializeField]
		private SO_MatchState _MatchState;
		[SerializeField]
		private SO_LobbyDetails _LobbyDetails;
		[SerializeField]
		private SO_IngredientContainerReference[] _IngredientContainerReferences;

		[Space, Header("Screens to switch to")]
		[SerializeField]
		private SO_Tag _LeaveMatchScreenTag;

	#endregion

	#region Life Cycle

		protected override void Awake() 
		{
			base.Awake();
			// Subscribing to dish cooked events of all players
			_DishCookedEvent.AddListener(OnDishCooked);
			_MatchStartedEvent.AddListener(OnMatchStarted);
			Initialize();
		}

	#endregion

	#region Member Functions

		private void OnMatchStarted(object data)
		{
			Initialize();
		}

		private void Initialize()
		{
			// Initializing the UI
			_LocalPlayerSlider.value = 0;
			_RemotePlayerSlider.value = 0;

			// FIXME: This is temporary as now there is considered to be only one dish
			_DishImage.sprite = _LobbyDetails.ChosenDishes[0].DishThumbnail; 

			// Generating Ingredient Images
			RemoveExistingIngredientImages();
			GenerateIngredientImages(_LobbyDetails.ChosenDishes[0]);
		}

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

		private void GenerateIngredientImages(SO_Dish chosenDish)
		{
			var recipeList = chosenDish.DishRecipe.IngredientsList;
			Dictionary<SO_Tag, List<SO_Tag>> ingredientStepsPair = new Dictionary<SO_Tag, List<SO_Tag>>();

			foreach(var cookingStep in recipeList)
			{
				if (!ingredientStepsPair.ContainsKey(cookingStep.Ingredient))
				{
					ingredientStepsPair[cookingStep.Ingredient] = new List<SO_Tag>();
				}
				ingredientStepsPair[cookingStep.Ingredient].Add(cookingStep.CookingMethod);
			}

			// Generating the ingredientImages and storing them in scriptable objects
			int i = 0;
			foreach (var cookingStep in ingredientStepsPair)
			{
				var ingredientContainer = Instantiate(_IngredientContainerPrefab, _CookingStepsContainer);
				ingredientContainer.Initialize(cookingStep.Key, cookingStep.Value.ToArray());
				_IngredientContainerReferences[i].Reference = ingredientContainer;
				++i;
			}
		}

		private void RemoveExistingIngredientImages()
		{
			RectTransform[] existingIngredientImages = _CookingStepsContainer.GetComponentsInChildren<RectTransform>();
			foreach (var ingredientImage in existingIngredientImages)
			{
				if (ingredientImage != _CookingStepsContainer)
				{
					Destroy(ingredientImage.gameObject);
				}
			}
		}

		public void OnQuitButtonPressed()
		{
			_UIManager.SetScreen(_LeaveMatchScreenTag);
		}

	#endregion
}