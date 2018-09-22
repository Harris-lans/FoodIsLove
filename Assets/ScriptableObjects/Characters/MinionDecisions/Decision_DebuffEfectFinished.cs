using System.Collections;
using System.Collections.Generic;
using PSM;
using UnityEngine;
using UnityEngine.AI;

public class Decision_DebuffEfectFinished : SO_ADecision 
{
    public override bool Decide(PluggableStateMachine pluggableStateMachine)
    {
        NavMeshAgent agent = pluggableStateMachine.gameObject.GetComponent<NavMeshAgent>();
		if (agent != null)
		{
			return agent.speed > 0 ? true : false;
		}
		return false;
    }
}
