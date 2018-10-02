using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="FoodIsLove/Level/Judge", fileName = "Judge")]
public class SO_Judge : ScriptableObject 
{
	[Header("Judge Details")]
	public string Name;
    public Sprite JudgeThumbnail;
	public SO_Dish[] PreferredDishes;
    public SO_GenericEvent AnnouncementEvent;

    public SO_Dish ChosenDish
    {
        get
        {
            return PreferredDishes[Random.Range(0, PreferredDishes.Length)];
        }
    }
}
