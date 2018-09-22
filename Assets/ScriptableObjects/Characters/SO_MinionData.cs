using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionData", menuName="FoodIsLove/Characters/MinionData")]
public class SO_MinionData : ScriptableObject 
{
	public Sprite Thumbnail;
	public GameObject MinionPrefab;
}
