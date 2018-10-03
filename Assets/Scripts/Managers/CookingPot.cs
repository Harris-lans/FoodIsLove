using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class CookingPot : MonoBehaviour 
{
	public static List<CookingPot> CookingPots;

	#region Member Variables

		// Events
		[Header("Events")]
		[SerializeField]
		private UnityEvent _DishHasBeenCookedEvent;
        [SerializeField]
        private SO_GenericEvent _IngredientWastedEvent;
		[SerializeField]
		private SO_GenericEvent _IngredientAddedToCookingPotEvent;
		[SerializeField]
		private SO_GenericEvent _DishCookedEvent;
		[SerializeField]
		private SO_GenericEvent _DishesCompletedEvent;

		[Header("Match Details")]
		[SerializeField]
		private SO_MatchState _MatchState;


		public Dictionary<SO_Dish, List<CookedIngredient>> DishesBeingPrepared;
		private SO_Dish _CurrentDishBeingCooked;
		private int _IndexOfCurrentDishBeingPrepared;
		private PhotonView _CookingPotOwner;
		private int _DishesCooked;
		private int _NumberOfIngredientsInPlace;

	#endregion
	

	#region Life Cycle

		private void Awake() 
		{
			DishesBeingPrepared = new Dictionary<SO_Dish, List<CookedIngredient>>();
			_IndexOfCurrentDishBeingPrepared = 0;
			_DishesCooked = 0;	
		}

		private void Start() 
		{
			foreach (var cookingStation in CookingStation.CookingStations)
			{
				// Subscribing to ingredient cooked event
				cookingStation.IngredientPickedUpEvent += AddCookedIngredient;
			}

			// Initializing the dishes to be prepared
			foreach(var expectedDish in _MatchState.ExpectedDishes)
			{
				DishesBeingPrepared[expectedDish] = new List<CookedIngredient>();
			}

			// Pointing to the dish currently being cooked
			_CurrentDishBeingCooked = DishesBeingPrepared.ElementAt(_IndexOfCurrentDishBeingPrepared).Key;
		}

		private void OnEnable() 
		{
			if(CookingPots == null)
			{
				CookingPots = new List<CookingPot>();
			}
			CookingPots.Add(this);
		}

		private void OnDisable() 
		{
			CookingPots.Remove(this);
		}

	#endregion

	#region Member Functions
		
		public void Initialize(PhotonView potOwner)
		{	
			// Setting the owner of the cooking pot
			_CookingPotOwner = potOwner;

			// Registering Cooking Pot
			_MatchState.RegisterCookingPot(potOwner.ViewID, this);
		}

		private void AddCookedIngredient(int playerWhoCooked, SO_Tag ingredient, SO_Tag cookingMethod)
		{
			// Checking if the cooking pot belongs to the player who cooked the ingredients
			if (playerWhoCooked != _CookingPotOwner.ViewID)
			{
				return;
			}

			CookedIngredient cookedIngredient = new CookedIngredient(ingredient, cookingMethod);
			DishesBeingPrepared[_CurrentDishBeingCooked].Add(cookedIngredient);

            // Checking if there is progress after adding ingredient to the pot
		    int preUpdateNumber = _NumberOfIngredientsInPlace;
			
		    UpdateDishStatus();

		    if (preUpdateNumber == _NumberOfIngredientsInPlace)
		    {
                // Letting the spawner know that the current ingredient cooked was not used properly and needs to be spawned again
		        _IngredientWastedEvent.Invoke(ingredient);
		    }
			else
			{
				_IngredientAddedToCookingPotEvent.Invoke(cookedIngredient);
			}
		}

		private void UpdateDishStatus()
		{
			// Checking if the dish is prepared
			int numberOfIngredientsInPot = 0;
			SO_Recipe recipeOfDishBeingPrepared = _CurrentDishBeingCooked.DishRecipe;
			
			foreach(var recipeInstruction in recipeOfDishBeingPrepared.IngredientsList)
			{
				foreach(var cookedIngredient in DishesBeingPrepared[_CurrentDishBeingCooked])
				{
					if (cookedIngredient == recipeInstruction)
					{
						++numberOfIngredientsInPot;
						break;
					}
				}
			}

			_NumberOfIngredientsInPlace = numberOfIngredientsInPot;
			_DishCookedEvent.Invoke(_CookingPotOwner.ViewID);

			if (numberOfIngredientsInPot == recipeOfDishBeingPrepared.IngredientsList.Length)
			{
				++_DishesCooked;
				_DishHasBeenCookedEvent.Invoke();
				
				// Checking if there are more dishes to be prepared and changing the pointer
				if (_IndexOfCurrentDishBeingPrepared + 2 < DishesBeingPrepared.Count)
				{
					++_IndexOfCurrentDishBeingPrepared;
					_CurrentDishBeingCooked = DishesBeingPrepared.ElementAt(_IndexOfCurrentDishBeingPrepared).Key;
				}
				else
				{
					// Player has completed all the dishes
					_DishesCompletedEvent.Invoke(_CookingPotOwner.ViewID);
				}
			}
		}

	#endregion

	#region Properties

		public float CurrentDishStatusFraction
		{
			get 
			{
				return (float)_NumberOfIngredientsInPlace / _CurrentDishBeingCooked.DishRecipe.IngredientsList.Length;
			}
		}

		public int DishesCompleted
		{
			get
			{
				return _DishesCooked;
			}
		}

	#endregion
}

[System.Serializable]
public struct CookedIngredient
{
	public SO_Tag Ingredient;
	public SO_Tag CookingMethod;

	public CookedIngredient(SO_Tag ingredient, SO_Tag cookingMethod)
	{
		Ingredient = ingredient;
		CookingMethod = cookingMethod;
	}

	public static bool operator== (CookedIngredient a, CookedIngredient b)
	{
		return (a.Ingredient == b.Ingredient && a.CookingMethod == b.CookingMethod);
	}

	public static bool operator!= (CookedIngredient a, CookedIngredient b)
	{
		return (a.Ingredient != b.Ingredient || a.CookingMethod != b.CookingMethod);
	}
}