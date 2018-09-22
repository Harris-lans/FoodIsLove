using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SO_Score", menuName = "FoodIsLove/SO_Score", order = 0)]
public class SO_Score : ScriptableObject 
{
	public float Score = 0;

	private void OnEnable() 
	{
		Score = 0;
	}

	public void IncreaseScore(float amountToIncrease)
	{
		Score += amountToIncrease;
	}
}
