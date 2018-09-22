using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;

[CreateAssetMenu(menuName = "PSM/GenericActions/PrintString", fileName = "PrintString")]
public class Action_PrintString : SO_AAction
{
	[SerializeField]
	private string _TextToPrint;

    public override void Act(PluggableStateMachine pluggableStateMachine)
    {
        Debug.Log(pluggableStateMachine.CurrentState.name + " : " + _TextToPrint);
    }
}
