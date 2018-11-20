using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "FoodIsLove/Dishes/NewRecipe")]
public class SO_Recipe : ScriptableObject 
{
    [Header("Create your own recipe")]
	public CookedIngredient[] IngredientsList;

    public Dictionary<SO_Tag, List<SO_Tag>> GetRecipeDictionary()
    {
        Dictionary<SO_Tag, List<SO_Tag>> recipeDictionary = new Dictionary<SO_Tag, List<SO_Tag>>();

        foreach (var ingredientStep in IngredientsList)
        {
            if (!recipeDictionary.ContainsKey(ingredientStep.Ingredient))
            {
                recipeDictionary[ingredientStep.Ingredient] = new List<SO_Tag>();
            }
            recipeDictionary[ingredientStep.Ingredient].Add(ingredientStep.CookingMethod);
        }

        return recipeDictionary; 
    }
}
