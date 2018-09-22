using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionButton : MonoBehaviour 
{
	#region Global Variables

		[SerializeField]
		private LocalMinionController _MinionController;

		[SerializeField]
		private SO_MinionSelectEventHandler _MinionSelectEventHandler;

	#endregion

	#region Life Cycles
		
		private void Start()
		{
			// Nothing to do here for now
		}

	#endregion

	#region Member Functions

		public void OnClickedButton()
		{
			_MinionSelectEventHandler.InvokeEvent(_MinionController);
		}

	#endregion
}
