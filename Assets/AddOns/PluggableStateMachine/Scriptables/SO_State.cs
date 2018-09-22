using UnityEngine;

namespace PSM
{
	[CreateAssetMenu(menuName="PSM/State", fileName = "State_")]
	public class SO_State : ScriptableObject 
	{
		[SerializeField]
		private SO_AAction[] EnterActions;

		[SerializeField]
		private SO_AAction[] UpdateActions;

		[SerializeField]
		private SO_AAction[] ExitActions;

		[Space, Tooltip("Transitions are OR-ed")]
		[SerializeField]
		private Transition[] _Transitions;

		public void EnterState(PluggableStateMachine pluggableStateMachine)
		{
			DoActions(EnterActions, pluggableStateMachine);
		}

		public void UpdateState(PluggableStateMachine pluggableStateMachine)
		{
			DoActions(UpdateActions, pluggableStateMachine);
			CheckTransitions(pluggableStateMachine);
		}

		public void ExitState(PluggableStateMachine pluggableStateMachine)
		{
			DoActions(ExitActions, pluggableStateMachine);
		}

		private void DoActions(SO_AAction[] actionsToDo, PluggableStateMachine pluggableStateMachine)
		{
			foreach (SO_AAction action in actionsToDo)
			{
				action.Act(pluggableStateMachine);
			}
		}

		private void CheckTransitions(PluggableStateMachine pluggableStateMachine)
		{
			foreach (var transition in _Transitions)
			{
				transition.CheckAndTransition(pluggableStateMachine);
			}
		}
	}
}
