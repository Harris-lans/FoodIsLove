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
		private Image _DishImage;
		[SerializeField]
		private RectTransform _CookingStepsContainer;

		[Header("UI Prefabs")]
		[SerializeField]
		private IngredientContainer _IngredientContainerPrefab;

		[Header("Events to listen to")]
		[SerializeField]
		private SO_GenericEvent _DishCookedEvent;

		[Header("Match Stats")]
		[SerializeField]
		private SO_MatchState _MatchState;

		[SerializeField]
		private SO_LobbyDetails _LobbyDetails;

	#endregion

	#region Life Cycle

		private void Start() 
		{
			// Subscribing to dish cooked events of all players
			//_DishCookedEvent.AddListener(OnDishCooked);
			
			// FIXME: This is temporary as now there is considered to be only one dish
			_DishImage.sprite = _LobbyDetails.ChosenDishes[0].DishThumbnail; 

			// Generating Ingredient Images
			GenerateIngredientImages(_LobbyDetails.ChosenDishes[0]);
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
		}

		private void GenerateIngredientImages(SO_Dish chosenDish)
		{
			Debug.Log(" Generating ingredient images" );
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

			// Generating thw ingredientImages
			foreach (var cookingStep in ingredientStepsPair)
			{
				var ingredientContainer = Instantiate(_IngredientContainerPrefab, _CookingStepsContainer);
				ingredientContainer.Initialize(cookingStep.Key, cookingStep.Value.ToArray());
			}
		}

	#endregion
}