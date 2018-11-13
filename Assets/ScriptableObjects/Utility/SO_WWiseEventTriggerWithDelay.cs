using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="FoodIsLove/WWise/WWiseEventTriggerWithDelay", fileName = "WWiseEventTriggerWithDelay")]
public class SO_WWiseEventTriggerWithDelay : ScriptableObject 
{
	public string EventName;
	public float DelayBeforeTriggeringEvent;
}
