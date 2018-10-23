using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName="FoodIsLove/Characters/HeroData")]
public class SO_HeroData : ScriptableObject 
{
	public Sprite Thumbnail;
	public HeroController HeroPrefab;
	public RectTransform HeroCard;
}
