using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="FoodIsLove/Level/Judge", fileName = "Judge")]
public class SO_Judge : ScriptableObject 
{
	[Header("Judge Details")]
	public string Name;
	public SO_Dish[] PreferredDishes;
}
