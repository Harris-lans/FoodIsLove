using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;

[CreateAssetMenu(menuName = "MinionPSM/Actions/Cook", fileName = "Cook")]
public class Action_Cook : SO_AAction
{
    public override void Act(PluggableStateMachine pluggableStateMachine)
    {
        // The local minion controller already knows about the target cooking station
        pluggableStateMachine.gameObject.GetComponent<LocalMinionController>().Cook(null);
    }
}
