using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ScoredEventHandler", menuName = "FoodIsLove/GameState/ScoredEventHandler")]
public class SO_ScoredEventHandler : ScriptableObject 
{
	[SerializeField]
	public ScoredEvent PlayerScoredEvent;


	[System.Serializable]
	public class ScoredEvent : UnityEvent<float> {}
}


