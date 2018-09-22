using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "FoodIsLove/Dishes/NewRecipe")]
public class SO_Recipe : ScriptableObject 
{
    [Header("Create your own recipe")]
	public CookedIngredient[] IngredientsList;
}
