using System.Collections;
using System.Collections.Generic;
using PSM;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "MinionPSM/Actions/Reset Speed", fileName = "Reset Minion Speed")]
public class Action_ResetSpeed : SO_AAction 
{
    public override void Act(PluggableStateMachine pluggableStateMachine)
    {
        // The minion controller already knows about the target cooking station
        pluggableStateMachine.gameObject.GetComponent<NavMeshAgent>().speed = 3.5f;
    }
}
