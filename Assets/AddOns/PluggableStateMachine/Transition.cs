using UnityEngine;

namespace PSM
{
	[System.Serializable]
	public class Transition 
	{
		[Tooltip("All Decisions are AND-ed")]
		[SerializeField]
		private SO_ADecision[] _Decisions;

		[SerializeField]
		private SO_State _TrueState;

		[SerializeField]
		private SO_State _FalseState;

		public void CheckAndTransition(PluggableStateMachine pluggableStateMachine)
		{
			foreach (var decision in _Decisions)
			{
				if (decision.Decide(pluggableStateMachine) == false)
				{
					pluggableStateMachine.ChangeState(_FalseState);
					return;
				}

				pluggableStateMachine.ChangeState(_TrueState);
			}
		}
	}
}
