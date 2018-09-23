using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;

[CreateAssetMenu(menuName = "MinionPSM/Actions/FollowHero", fileName = "FollowHero")]
public class Action_FollowHero : SO_AAction
{
    public override void Act(PluggableStateMachine pluggableStateMachine)
    {
		//pluggableStateMachine.gameObject.GetComponent<LocalMinionController>().MoveToHero();	
    }
}
