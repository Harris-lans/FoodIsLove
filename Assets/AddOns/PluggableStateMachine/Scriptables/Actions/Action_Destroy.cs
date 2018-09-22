using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;

[CreateAssetMenu(menuName = "PSM/GenericActions/Destroy", fileName = "Destroy")]
public class Action_Destroy : SO_AAction
{
	[SerializeField]
	private int _TimeBeforeDestroy;

    public override void Act(PluggableStateMachine pluggableStateMachine)
    {
		// Destroying the gameObject that the state machine is attached to
        Destroy(pluggableStateMachine.gameObject, _TimeBeforeDestroy);
    }
}
