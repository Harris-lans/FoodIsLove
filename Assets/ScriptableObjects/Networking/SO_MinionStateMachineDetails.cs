using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;

[CreateAssetMenu(fileName = "MinionStateMachineDetails", menuName="FoodIsLove/Characters/MinionStateMachineDetails")]
public class SO_MinionStateMachineDetails : ScriptableObject 
{
	[Header("State Machine Details for Minions")]
	public SO_State DefaultState;
	public SO_State MoveToCookingStationState;
	public SO_State StunState;
}
