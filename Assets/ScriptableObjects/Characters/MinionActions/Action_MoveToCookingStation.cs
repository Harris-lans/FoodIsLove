using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;

[CreateAssetMenu(menuName = "MinionPSM/Actions/MoveToCookingStation", fileName = "MoveToCookingStation")]
public class Action_MoveToCookingStation : SO_AAction 
{

    public override void Act(PluggableStateMachine pluggableStateMachine)
    {
		// The position we pass does not matter as this is a local minion controller
        pluggableStateMachine.gameObject.GetComponent<LocalMinionController>().MoveToTargetGrid(new GridPosition());
    }
}
