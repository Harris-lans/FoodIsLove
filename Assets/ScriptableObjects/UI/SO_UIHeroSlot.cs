using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UI_HeroSlot", menuName="FoodIsLove/UI/UI_HeroSlot")]
public class SO_UIHeroSlot : ScriptableObject 
{
	public Sprite ThumbnailToDisplay;
	public string Name;
	public HeroController Hero;
}
