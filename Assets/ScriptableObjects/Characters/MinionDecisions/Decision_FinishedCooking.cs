using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;

[CreateAssetMenu(menuName = "MinionPSM/Decisions/FinishedCooking", fileName = "FinishedCooking")]
public class Decision_FinishedCooking : SO_ADecision
{
    public override bool Decide(PluggableStateMachine pluggableStateMachine)
    {
        //LocalMinionController minionController = pluggableStateMachine.gameObject.GetComponent<LocalMinionController>();
		//if (minionController != null)
		//{
		//	return minionController.HasFinishedCooking();
		//}
		return false;
    }
}
