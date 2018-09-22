using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "FoodIsLove/LevelData/IngredientSpawnDetails", fileName = "IngredientSpawnDetails")]
public class SO_IngredientSpawnData : ScriptableObject
{
    #region Member Variables

        public Dictionary<SO_Tag, int> IngredientsToSpawn;
        
        [Space, Header("Events")]
        [SerializeField]
        private SO_GenericEvent _UncookedIngredientDestroyedEventHandler;

    #endregion

    #region Member Functions

        public void Initialize()
        {
            IngredientsToSpawn = new Dictionary<SO_Tag, int>();
            _UncookedIngredientDestroyedEventHandler.AddListener(OnUncookedIngredientDestroyed);
        }

        public void AddRecipeIngredientsForSpawning(SO_Recipe recipe)
        {
            foreach (var ingredientPreparationMethod in recipe.IngredientsList)
            {
                AddIngredientToSpawnable(ingredientPreparationMethod.Ingredient);
            }
        }

        public void AddIngredientToSpawnable(SO_Tag ingredientTag)
        {
            Debug.LogFormat("Ingredient added to spawnable: {0}", ingredientTag.name);
            // Adding the ingredient so that it spawns
            if (IngredientsToSpawn.ContainsKey(ingredientTag))
            {
                ++IngredientsToSpawn[ingredientTag];
            }
            else
            {
                IngredientsToSpawn[ingredientTag] = 1;
            }
        }

        public void RemoveIngredientFromSpawnable(SO_Tag ingredientTag)
        {
            // Removing the ingredient so that it does not spawn
            if (IngredientsToSpawn.ContainsKey(ingredientTag))
            {
                --IngredientsToSpawn[ingredientTag];
                if (IngredientsToSpawn[ingredientTag] <= 0)
                {
                    IngredientsToSpawn.Remove(ingredientTag);
                }
            }
        }

        public SO_Tag ChooseIngredientToSpawn()
        {
            foreach (var ingredientCount in IngredientsToSpawn)
            {
                Debug.LogFormat("{0}  :  {1}", ingredientCount.Key.name, ingredientCount.Value);
            }

            if (IngredientsToSpawn == null || IngredientsToSpawn.Count == 0)
            {
                return null;
            }

            int randomIndex = Random.Range(0, IngredientsToSpawn.Count);
            KeyValuePair<SO_Tag, int> ingredientCountData = IngredientsToSpawn.ElementAt(randomIndex);
            SO_Tag ingredientToSpawn = ingredientCountData.Key;
            RemoveIngredientFromSpawnable(ingredientToSpawn);
            return ingredientToSpawn;
        }

        public void OnUncookedIngredientDestroyed(object data)
        {
            SO_Tag ingredientTag = (SO_Tag)data;
            AddIngredientToSpawnable(ingredientTag);
        }

    #endregion 
}
