using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData", menuName="FoodIsLove/Level/SpawnData")]
public class SO_SpawnData : ScriptableObject 
{
	
}

[System.Serializable]
public struct TeamSpawnData
{
	public GridPosition[] SpawnPoints;
}