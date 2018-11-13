using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientsData", menuName = "FoodIsLove/GameData/IngredientsData")]
public class SO_IngredientData : ScriptableObject 
{
	public IngredientData[] IngredientsData;

	public Sprite GetIngredientIcon(SO_Tag ingredient)
	{
		Sprite icon = null;
		foreach (var ingredientData in IngredientsData)
		{
			if (ingredientData.Ingredient == ingredient)
			{
				icon = ingredientData.Icon;
				break;
			}
		}
		return icon;
	}
}

[System.Serializable]
public struct IngredientData
{
	public SO_Tag Ingredient;
	public Sprite Icon;
}
