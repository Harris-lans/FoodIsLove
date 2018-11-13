using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingStepsData", menuName = "FoodIsLove/GameData/CookingStepsData")]
public class SO_CookingStepsData : ScriptableObject 
{
	public CookingStepData[] CookingStepsData;

	public Sprite GetCookingStepSprite(SO_Tag cookingStep)
	{
		Sprite icon = null;
		foreach (var cookingStepData in CookingStepsData)
		{
			if (cookingStepData.CookingStep == cookingStep)
			{
				icon = cookingStepData.Icon;
				break;
			}
		}
		return icon;
	}
}

[System.Serializable]
public struct CookingStepData
{
	public SO_Tag CookingStep;
	public Sprite Icon;
}