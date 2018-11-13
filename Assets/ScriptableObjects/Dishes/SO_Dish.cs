using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewDish", menuName = "FoodIsLove/Dishes/Dish")]
public class SO_Dish : ScriptableObject 
{
     [Header("Create your own dish")]
	 public SO_Recipe DishRecipe;
	 public Sprite DishThumbnail;
}
