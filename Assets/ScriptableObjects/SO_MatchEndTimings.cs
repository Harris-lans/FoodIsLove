using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatchEndTimings", menuName="FoodIsLove/TimingData/MatchEndTimings")]
public class SO_MatchEndTimings : ScriptableObject 
{
	public float TimeBeforeTriggerinigSlowMotion;
	public float SlowMotionTime;
	public float TimeToComeBackToNormalSpeed;
	public float TimeBeforeShowingEndScreen;
	public float TimeBeforeAnnouncing; 
}
