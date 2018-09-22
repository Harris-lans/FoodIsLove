using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;

[CreateAssetMenu(menuName = "PSM/GenericActions/PlayWWiseSound", fileName = "PlayWWiseSound")]
public class Action_PlaySound : SO_AAction
{
	[SerializeField]
	private string _WWiseEventToRaise;

    public override void Act(PluggableStateMachine pluggableStateMachine)
    {
        // TODO: Invoke WWise Event
    }
}
