using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="FoodIsLove/GameState/DishCookedEvent", fileName = "DishCookedEvent")]
public class SO_DishCookedEvent : ScriptableObject 
{
	private Dictionary<int, List<DishCookedAction>> _DishCookedEvents;

	public void Intialize()
	{
		_DishCookedEvents = null;
	}

	public void SubscribeToDishCookedEvent(int idOfPlayerWhoCooked, DishCookedAction action)
	{
		if (!_DishCookedEvents.ContainsKey(idOfPlayerWhoCooked))
		{
			_DishCookedEvents[idOfPlayerWhoCooked] = new List<DishCookedAction>();
		}

		_DishCookedEvents[idOfPlayerWhoCooked].Add(action);
	}

	public void InvokeEvent(int idOfPlayerWhoCooked, SO_Dish dish)
	{
		if (!_DishCookedEvents.ContainsKey(idOfPlayerWhoCooked))
		{
			return;
		}

		// Invoking the events specific to one player
		foreach(var dishCookedEvent in _DishCookedEvents[idOfPlayerWhoCooked])
		{
			dishCookedEvent(dish);
		}
	}
}

public delegate void DishCookedAction(SO_Dish dish);
