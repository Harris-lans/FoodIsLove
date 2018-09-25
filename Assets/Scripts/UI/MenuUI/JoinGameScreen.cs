using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameScreen : UIScreen 
{
	#region Member Variables

		[Space, Header("Input Fields for Network Configuration")]
		[SerializeField]
		private InputField _JoinGameTextField;
		[SerializeField]
		private Button _CreateRoomButton; 
		
		[Header("Screen Transition Details: ")]
		[SerializeField]
		private SO_Tag _ScreenToChangeToOnJoinGame;

    #endregion

    #region Life Cycle


	#endregion

	#region Member Functions

		public void JoinGame()
		{
			_UIManager.SetScreen(_ScreenToChangeToOnJoinGame);
		}

	#endregion
}
