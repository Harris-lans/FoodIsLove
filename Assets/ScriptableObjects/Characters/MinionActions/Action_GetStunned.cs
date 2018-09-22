using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "MinionPSM/Actions/Stunned", fileName = "Stunned")]
public class Action_GetStunned : SO_AAction 
{

    public override void Act(PluggableStateMachine pluggableStateMachine)
    {
        // The minion controller already knows about the target cooking station
        //pluggableStateMachine.gameObject.GetComponent<MinionController>().GetStunned(2);
    }

}
