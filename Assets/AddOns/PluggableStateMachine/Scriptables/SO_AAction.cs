using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSM
{
	public abstract class SO_AAction : ScriptableObject 
	{
		public abstract void Act(PluggableStateMachine pluggableStateMachine);
	}
}
