using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroesList", menuName = "FoodIsLove/GameData/HeroesList")]
public class SO_HeroList : ScriptableObject 
{
	public SO_HeroData[] Heroes;
}
