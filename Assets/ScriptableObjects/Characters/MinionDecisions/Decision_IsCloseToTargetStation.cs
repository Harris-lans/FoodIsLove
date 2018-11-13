using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;

[CreateAssetMenu(menuName="MinionPSM/Decisions/IsCloseToTargetStation", fileName = "IsCloseToTargetStation_")]
public class Decision_IsCloseToTargetStation : SO_ADecision
{
	[SerializeField]
	private float _MininmumDistance = 2.0f;

    public override bool Decide(PluggableStateMachine pluggableStateMachine)
    {
        /*
        LocalMinionController minionController = pluggableStateMachine.gameObject.GetComponent<LocalMinionController>();
		if (minionController != null)
		{
			return minionController.IsCloseToCookingStation(_MininmumDistance);
		}*/
		return false;
    }
}
