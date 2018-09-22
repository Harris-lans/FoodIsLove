using System;
using UnityEngine;

namespace PSM
{
	public class PluggableStateMachine 
	{
		public GameObject gameObject { get; private set; }
		public SO_State CurrentState { get; private set; }

		private bool _IsActive = false;

		public PluggableStateMachine(GameObject ownerGameObject, SO_State defaultState)
		{
			gameObject = ownerGameObject;
			ChangeState(defaultState);
			_IsActive = true;
		}

		public void ChangeState(SO_State newState)
		{
			if (newState == null)
			{
				return;
			}

			if (CurrentState != null)
			{
				// Performing Exit Actions
				CurrentState.ExitState(this);
			}

			CurrentState = newState;
			
			// Performing Enter actions
			CurrentState.EnterState(this);
		}

		public void OnUpdate()
		{
			if (!_IsActive || CurrentState == null)
			{
				return;
			}

			CurrentState.UpdateState(this);
		}
	}
}
