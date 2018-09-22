using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UI_MinionSlot", menuName="FoodIsLove/UI/UI_MinionSlot")]
public class SO_UIMinionSlot : ScriptableObject 
{
	public Sprite ThumbnailToDisplay;
	public string Name;
	public IngredientMinion Ingredient;

	public void Initialize()
	{
		ThumbnailToDisplay = null;
		Name = null;
		Ingredient = null;
	}
}
