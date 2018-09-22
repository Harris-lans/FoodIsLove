using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSM
{
	public abstract class SO_ADecision : ScriptableObject 
	{
		public abstract bool Decide(PluggableStateMachine pluggableStateMachine);
	}
}
