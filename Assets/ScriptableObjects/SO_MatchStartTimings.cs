using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatchStartTimings", menuName="FoodIsLove/TimingData/MatchStartTimings")]
public class SO_MatchStartTimings : ScriptableObject 
{
	public float TimeDelayBeforeSpawningThePlayers;
	public float TimeDelayBeforeDisplayingTheMatchCountDown;
	public float TimeDelayBeforeShowingMap;
}
